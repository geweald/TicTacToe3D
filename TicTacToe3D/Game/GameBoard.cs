using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
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
                new SolidColorBrush(Color.FromArgb(255, 255, 0, 0)),
                new SolidColorBrush(Color.FromArgb(255, 0, 255, 0)),
                new SolidColorBrush(Color.FromArgb(255, 255, 255, 0)),
                new SolidColorBrush(Color.FromArgb(255, 255, 0, 255)),
                new SolidColorBrush(Color.FromArgb(255, 0, 255, 255))
            };
        #endregion

        private readonly GameField[,,] _gameFields;
        private List<GameField> _gameFieldsList;
        private readonly int _size;
        private readonly Transform3DTool _transform3DTool;
        private readonly Canvas _canvas;

        private int _highlightedLayer;
        public int HighlightedField { get; private set; }


        public GameBoard(ushort size, Canvas canvas)
        {
            FreezeBrushes();

            _size = size > 5 ? 5 : size;
            _transform3DTool = Transform3DTool.Instance();
            _canvas = canvas;
            _highlightedLayer = 0;
            HighlightedField = 0;

            _gameFields = new GameField[_size, _size, _size];
            MakeCubes();

            RotateZ(90);
            RotateY(-90);
            RotateX(70);
            RotateY(-20);
        }

        public void DrawGameBoard()
        {
            _transform3DTool.SetCanvasSize(_canvas.ActualWidth, _canvas.ActualHeight);
            CubesListSortZDesc();
            var innerCanvas = new Canvas { CacheMode = new BitmapCache(2) };
            _canvas.Children.Clear();

            //var watch = System.Diagnostics.Stopwatch.StartNew();
            foreach (var gfield in _gameFieldsList)
            {
                DrawGameField(gfield, innerCanvas);
            }
            //watch.Stop();
            //var elapsedMs = watch.ElapsedTicks;
            //Console.WriteLine("DRAW TICKS: " + elapsedMs);

            _canvas.Children.Add(innerCanvas);
        }

        public void ChangeLayer(bool next = true)
        {
            if (next)
            {
                if (_highlightedLayer < _size - 1)
                {
                    ++_highlightedLayer;
                    HighlightedField += _size * _size;
                    DrawGameBoard();
                }
            }
            else
            {
                if (_highlightedLayer > 0)
                {
                    --_highlightedLayer;
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

        private void MakeCubes()
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
                var points = Transform3DTool.Instance().TransformPointsTo2D(cubeFaces[i]);
                var p = new Polygon
                {
                    Stroke = _strokeBrushes[gameField.Layer],
                    Points = new PointCollection(points),
                    Fill = gameField.Layer == _highlightedLayer
                        ? _highlightBrushes[gameField.Layer]
                        : _normalBrushes[gameField.Layer],
                };
                if (gameField.FieldNr == HighlightedField)
                {
                    p.Stroke = Brushes.White;
                    p.StrokeThickness = 2;
                }
                if ((i == 3) && gameField.Marked)
                {
                    var e = CubesSphere(gameField.Cube, gameField.PlayerColor);
                    canvas.Children.Add(e);
                }
                canvas.Children.Add(p);
            }
        }

        private Ellipse CubesSphere(Cube cube, SolidColorBrush color)
        {
            var center = cube.Center();
            var centerAndD = new Point3D(center.X + cube.Size, center.Y, center.Z);
            var elc = _transform3DTool.TransformPointTo2D(center);
            var els = _transform3DTool.TransformPointTo2D(centerAndD);
            var d = Math.Sqrt(Math.Pow(elc.X - els.X, 2) + Math.Pow(elc.Y - els.Y, 2)) * 0.8;
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
            var layerStart = _highlightedLayer * layerItems;
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
            var layerStart = _highlightedLayer * layerItems;
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
            var layerStart = _highlightedLayer * layerItems;
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
            var layerStart = _highlightedLayer * layerItems;
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
            _highlightedLayer = 0;
            HighlightedField = 0;
            DrawGameBoard();
        }
    }
}
