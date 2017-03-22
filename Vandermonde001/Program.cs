using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vandermonde001
{
    class Program
    {
        static void Main(string[] args)
        {
            // テスト関数の係数
            double[] testCoef = new double[]
            {
                0.1,
                3.0,
                0.003,
                0.0,
            };
            // 誤差係数
            double randomCoef = 0.3;

            // 出力
            OutputCoef(testCoef);

            // 停止位置のログ
            StopRecord stopDistance = GetTestDistance(testCoef, randomCoef);
            //StopRecord stopDistance = new StopRecord();
            //stopDistance.Add(10, 10);
            //stopDistance.Add(20, 20);
            //stopDistance.Add(30, 30);
            //stopDistance.Add(40, 40);

            // 近似式の次数
            int degree = 3;

            // 処理
            var coefficient = PolyFit(stopDistance, degree);

            // 出力
            OutputCoef(coefficient);
        }

        /// <summary>
        /// テスト用のデータを生成する。
        /// </summary>
        /// <returns></returns>
        private static StopRecord GetTestDistance(double[] coef, double randomCoef, int randomSeed = 12342347)
        {
            // ノイズ
            var rand = new Random(randomSeed);

            var stopDistance = new StopRecord();
            for(int i = 10; i <= 100; i += 10) // 測定条件
            {
                for (int j = 0; j < 5; j++) // 試行回数
                {
                    var r = (rand.NextDouble() - 0.5) * randomCoef; // 人工的なノイズ
                    stopDistance.Add(i, TestFunction(coef, i) + r);
                }
            }
            return stopDistance;
        }


        /// <summary>
        /// テスト関数
        /// </summary>
        /// <param name="x">X軸</param>
        /// <param name="r">ノイズ</param>
        /// <returns></returns>
        private static double TestFunction(double[] coef, double x)
        {
            double v = 0.0;
            for (int i = 0; i < coef.Length; i++ )
            {
                v += Math.Pow(x, i) * coef[i];
            }
            return v;
        }

        /// <summary>
        /// テスト関数のログ出力
        /// </summary>
        private static void OutputCoef(double [] coef)
        {
            string msg = "係数";
            for (int i = 0; i < coef.Length; i++)
            {
                msg += string.Format(" [{0}]:{1:F7},", i, coef[i]);
            }
            Console.WriteLine(msg);
        }

        /// <summary>
        /// 多項式の近似式を求める
        /// </summary>
        /// <param name="stopDistance"></param>
        /// <returns></returns>
        public static double[] PolyFit(StopRecord stopDistance, int degree)
        {
            if (stopDistance == null)
                throw new ArgumentNullException();

            var x = stopDistance.SpeedArray;
            var y = stopDistance.DistanceArray;

            // 多項式の近似式を求める
            double[] p = Polyfit(x, y, degree);
            return p;
        }

        /// <summary>
        /// 近似式を求める。
        /// </summary>
        /// <param name="x">1軸</param>
        /// <param name="y">2軸</param>
        /// <param name="degree">次数</param>
        /// <returns>係数の配列</returns>
        public static double[] Polyfit(double[] x, double[] y, int degree)
        {
            if(x.Length == 0 || y.Length == 0 || x.Length != y.Length)
                return new double[0];

            // Vandermonde matrix
            var v = new DenseMatrix(x.Length, degree + 1);
            for (int i = 0; i < v.RowCount; i++)
            {
                for (int j = 0; j <= degree; j++)
                {
                    v[i, j] = Math.Pow(x[i], j);
                }
            }
            var yv = new DenseVector(y).ToColumnMatrix();
            var qr = v.QR();
            // Math.Net doesn't have an "economy" QR, so:
            // cut R short to square upper triangle, then recompute Q
            var r = qr.R.SubMatrix(0, degree + 1, 0, degree + 1);
            var q = v.Multiply(r.Inverse());
            var p = r.Inverse().Multiply(q.TransposeThisAndMultiply(yv));
            return p.Column(0).ToArray();
        }        

    }
}
