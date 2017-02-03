using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CalibrateStop.Model
{
    /// <summary>
    /// 係数データ
    /// </summary>
    public class ApproxCoefficient : ObservableObject
    {
        public ApproxCoefficient()
        {
        }


        private List<ApproxCoefficientItem> items;

        /// <summary>
        /// アイテム
        /// </summary>
        public List<ApproxCoefficientItem> Items
        {
            get
            {
                if (items == null)
                    items = new List<ApproxCoefficientItem>();
                return items;
            }
        }

        public List<Point> Points
        {
            get
            {
                var list = new List<Point>();
                if (start == end)
                    return list;

                for(double i = start; i <= end; i += step)
                {
                    double val = 0;
                    foreach(var item in Items)
                    {
                        val += item.Coefficient * Math.Pow(i, item.Degree);
                    }
                    list.Add(new Point(i, val));
                }
                return list;
            }
        }

        private double start = 0;

        public double Start
        {
            get
            {
                return start;
            }
            set
            {
                start = value;
                RaisePropertyChanged(() => Start);
            }
        }

        private double end = 50;

        public double End
        {
            get
            {
                return end;
            }
            set
            {
                end = value;
                RaisePropertyChanged(() => End);
            }
        }
        private double step = 1;

        public double Step
        {
            get
            {
                return step;
            }
            set
            {
                step = value;
                RaisePropertyChanged(() => Step);
            }
        }

        /// <summary>
        /// 開始点情報を生成する。
        /// </summary>
        /// <param name="coefficient">距離の列</param>
        /// <param name="start">開始点</param>
        /// <param name="end">終了点</param>
        /// <param name="step">ステップ</param>
        /// <returns>停止位置情報</returns>
        public static ApproxCoefficient Create(double[] coefficient, double start, double end, double step)
        {
            if (coefficient == null)
                throw new ArgumentNullException();

            var newObj = new ApproxCoefficient()
                {
                    Start = start,
                    End = end,
                    Step = step,
                };

            for(int i = 0; i < coefficient.Length; i++)
            {
                newObj.Items.Add(new ApproxCoefficientItem() {  
                    Degree = i, 
                    Coefficient = coefficient[i],
                });
            }

            return newObj;
        }
    }

    public class ApproxCoefficientItem : ObservableObject
    {
        private double degree;

        /// <summary>
        /// 指数
        /// </summary>
        public double Degree        
        {
            get
            {
                return degree;
            }
            set
            {
                degree = value;
                RaisePropertyChanged(() => Degree);
            }
        }
        
        private double coefficient;

        /// <summary>
        /// 係数
        /// </summary>
        public double Coefficient        
        {
            get
            {
                return coefficient;
            }
            set
            {
                coefficient = value;
                RaisePropertyChanged(() => Coefficient);
            }
        }
        


        
    }
}
