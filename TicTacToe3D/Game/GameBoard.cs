using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using TicTacToe3D.Drawing;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace TicTacToe3D.Game
{
    internal class GameBoard
    {
        #region BRUSHES
        private readonly SolidColorBrush[] _normalBrushes = {
                new SolidColorBrush(Color.FromArgb(20, 255, 0, 0)),
                new SolidColorBrush(Color.FromArgb(20, 0, 255, 0)),
                new SolidColorBrush(Color.FromArgb(20, 255, 255, 0)),
                new SolidColorBrush(Color.FromArgb(20, 255, 0, 255)),
                new SolidColorBrush(Color.FromArgb(20, 0, 255, 255))
            };
        private readonly SolidColorBrush[] _highlightBrushes = {
                new SolidColorBrush(Color.FromArgb(55, 255, 0, 0)),
                new SolidColorBrush(Color.FromArgb(55, 0, 255, 0)),
                new SolidColorBrush(Color.FromArgb(55, 255, 255, 0)),
                new SolidColorBrush(Color.FromArgb(55, 255, 0, 255)),
                new SolidColorBrush(Color.FromArgb(55, 0, 255, 255))
            };
        private readonly SolidColorBrush[] _strokeBrushes = {
                new SolidColorBrush(Color.FromArgb(95, 255, 0, 0)),
                new SolidColorBrush(Color.FromArgb(95, 0, 255, 0)),
                new SolidColorBrush(Color.FromArgb(95, 255, 255, 0)),
                new SolidColorBrush(Color.FromArgb(95, 255, 0, 255)),
                new SolidColorBrush(Color.FromArgb(95, 0, 255, 255))
            };

        private readonly SolidColorBrush _highlightStroke =
            Brushes.White;
        #endregion

        private readonly GameField[,,] _gameFields;
        private readonly int _size;
        private readonly Transform3DTool _transform3DTool;
        private readonly Canvas _canvas;
        private readonly Game _game;

        private List<GameField> _gameFieldsList;

        public int HighlightedLayer { get; private set; }
        public int HighlightedField { get; private set; }


        public GameBoard(ushort size, Canvas canvas, Game game = null)
        {
            FreezeBrushes();

            _size = size;
            _canvas = canvas;
            _game = game;
            _transform3DTool = new Transform3DTool();
            _gameFields = new GameField[_size, _size, _size];
            _transform3DTool.SetZoom(3 * _size);

            HighlightedLayer = 0;
            HighlightedField = 0;

            GenerateFields();
            TransformCubes();
        }

        public void DrawGameBoard()
        {
            _transform3DTool.SetCanvasSize(_canvas.ActualWidth, _canvas.ActualHeight);
            _canvas.Children.Clear();

            CubesListSortZDesc();

            var innerCanvas = new Canvas { CacheMode = new BitmapCache() };

            var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var gfield in _gameFieldsList)
            {
                DrawGameField(gfield, innerCanvas);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("DRAW TICKS: " + elapsedMs);

            _canvas.Children.Add(innerCanvas);
            _transform3DTool.ResetRotation();
        }

        public void ChangeLayer(bool next = true)
        {
            if (next)
            {
                if (HighlightedLayer < _size - 1)
                {
                    ++HighlightedLayer;
                    HighlightedField += _size * _size;
                    DrawGameBoard();
                }
            }
            else
            {
                if (HighlightedLayer > 0)
                {
                    --HighlightedLayer;
                    HighlightedField -= _size * _size;
                    DrawGameBoard();
                }
            }
        }

        public void ChangeField(int direction)
        {
            switch (direction)
            {
                case 0:
                    ChangeHighlightedFieldLeft();
                    break;
                case 1:
                    ChangeHighlightedFieldUp();
                    break;
                case 2:
                    ChangeHighlightedFieldRight();
                    break;
                case 3:
                    ChangeHighlightedFieldDown();
                    break;
            }
        }

        public void RotateX(double a)
        {
            _transform3DTool.RotateX(a);
            TransformCubes();
            DrawGameBoard();
        }

        public void RotateY(double a)
        {
            _transform3DTool.RotateY(a);
            TransformCubes();
            DrawGameBoard();
        }

        public void RotateXY(double x, double y)
        {
            _transform3DTool.RotateXY(x, y);
            TransformCubes();
            DrawGameBoard();
        }

        public void RotateZ(double a)
        {
            _transform3DTool.RotateZ(a);
            TransformCubes();
            DrawGameBoard();
        }

        public void Zoom(double zoom)
        {
            _transform3DTool.Zoom(zoom);
            DrawGameBoard();
        }

        private void TransformCubes()
        {
            foreach (var gf in _gameFields)
                gf.Cube.Transform(_transform3DTool);
        }

        private void GenerateFields()
        {
            _gameFieldsList = new List<GameField>();

            var margin = 0.1;
            var x = -(_size + (_size - 1) * margin) / 2.0;
            var y = x;
            var z = x;

            var nr = 0;
            for (var i = 0; i < _size; i++)
                for (var j = 0; j < _size; j++)
                    for (var k = 0; k < _size; k++)
                    {
                        var gf = new GameField(
                            new[] { x + i + i * margin, y + j + j * margin, z + k + k * margin }, i, nr++);
                        _gameFields[i, j, k] = gf;
                        _gameFieldsList.Add(gf);
                    }
        }

        private void DrawGameField(GameField gameField, Canvas canvas)
        {
            var cubeFaces = gameField.Cube.CubeFaces();
            for (var i = 0; i < cubeFaces.Count; ++i)
            {
                var points = _transform3DTool.TransformPointsTo2D(cubeFaces[i]);
                var p = new Polygon
                {
                    Stroke = _strokeBrushes[gameField.Layer],
                    StrokeMiterLimit = 1,
                    Points = new PointCollection(points),
                    Fill = _normalBrushes[gameField.Layer],
                    IsHitTestVisible = false
                };
                if (_game != null)
                    AddGameInteractions(gameField, p);
                canvas.Children.Add(p);
            }
            if (gameField.Marked)
            {
                var e = CubesSphere(gameField.Cube, gameField.PlayerColor);
                canvas.Children.Insert(canvas.Children.Count - 3, e);
            }
        }

        private void AddGameInteractions(GameField gameField, Polygon polygon)
        {
            if (gameField.FieldNr == HighlightedField)
            {
                polygon.Stroke = _highlightStroke;
                polygon.StrokeThickness = 2;
            }
            if (gameField.Layer == HighlightedLayer)
            {
                polygon.Fill = _highlightBrushes[gameField.Layer];
                polygon.IsHitTestVisible = true;
                polygon.MouseEnter += (sender, args) =>
                {
                    if (HighlightedField == gameField.FieldNr
                        || args.RightButton == MouseButtonState.Pressed)
                        return;
                    HighlightedField = gameField.FieldNr;
                    DrawGameBoard();
                };
                polygon.MouseDown += (sender, args) =>
                {
                    if (args.LeftButton == MouseButtonState.Pressed)
                        _game.MakeMove();
                };
            }
        }

        private Ellipse CubesSphere(Cube cube, SolidColorBrush color)
        {
            var center = cube.Center();
            var centerAndD = new Point3D(center.X + cube.Size, center.Y, center.Z);
            var elc = _transform3DTool.TransformPointTo2D(center);
            var els = _transform3DTool.TransformPointTo2D(centerAndD);
            var d = Math.Sqrt(Math.Pow(elc.X - els.X, 2) + Math.Pow(elc.Y - els.Y, 2)) * 0.8;
            //            var fill = new RadialGradientBrush
            //            {
            //                GradientOrigin = new Point(0.7, 0.3),
            //                GradientStops = new GradientStopCollection
            //                {
            //                    new GradientStop(Colors.White, 0),
            //                    new GradientStop(
            //                        color.Color == Colors.White
            //                        ? Colors.Gray
            //                        : color.Color,
            //                        1)
            //                }
            //            };
            //            fill.Freeze();
            var e = new Ellipse
            {
                Width = d,
                Height = d,
                Fill = color
            };
            Canvas.SetTop(e, elc.Y - d / 2);
            Canvas.SetLeft(e, elc.X - d / 2);
            return e;
        }

        private void CubesListSortZDesc()
        {
            _gameFieldsList = _gameFieldsList.OrderByDescending(gf => gf.Cube.Center().Z).ToList();
        }

        private void ChangeHighlightedFieldLeft()
        {
            var layerItems = _size * _size;
            var layerStart = HighlightedLayer * layerItems;
            var newField = HighlightedField - 1;
            if ((newField >= layerStart) && (HighlightedField % _size > 0))
            {
                HighlightedField = newField;
                DrawGameBoard();
            }
        }

        private void ChangeHighlightedFieldRight()
        {
            var layerItems = _size * _size;
            var layerStart = HighlightedLayer * layerItems;
            var layerStop = layerStart + layerItems;
            var newField = HighlightedField + 1;
            if ((newField < layerStop) && (newField % _size > 0))
            {
                HighlightedField = newField;
                DrawGameBoard();
            }
        }

        private void ChangeHighlightedFieldUp()
        {
            var layerItems = _size * _size;
            var layerStart = HighlightedLayer * layerItems;
            var layerStop = layerStart + layerItems;
            var newField = HighlightedField + _size;
            if (newField < layerStop)
            {
                HighlightedField = newField;
                DrawGameBoard();
            }
        }

        private void ChangeHighlightedFieldDown()
        {
            var layerItems = _size * _size;
            var layerStart = HighlightedLayer * layerItems;
            var newField = HighlightedField - _size;
            if (newField >= layerStart)
            {
                HighlightedField = newField;
                DrawGameBoard();
            }
        }

        private void FreezeBrushes()
        {
            foreach (var solidColorBrush in _normalBrushes)
                solidColorBrush.Freeze();
            foreach (var solidColorBrush in _highlightBrushes)
                solidColorBrush.Freeze();
            foreach (var solidColorBrush in _strokeBrushes)
                solidColorBrush.Freeze();
            _highlightStroke.Freeze();
        }

        public bool MarkField(int field, SolidColorBrush playerColor)
        {
            var highlightedGameField = _gameFieldsList.SingleOrDefault(gf => gf.FieldNr == field);
            if (highlightedGameField == null) return false;
            var result = highlightedGameField.Mark(playerColor);
            if (result) DrawGameBoard();
            return result;
        }

        public void Clear()
        {
            foreach (var gameField in _gameFields)
                gameField.Clear();
            HighlightedLayer = 0;
            HighlightedField = 0;
            DrawGameBoard();
        }
    }
}
