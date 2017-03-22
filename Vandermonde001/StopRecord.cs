using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Vandermonde001
{
    /// <summary>
    /// 停止位置
    /// </summary>
    public class StopRecord : GalaSoft.MvvmLight.ObservableObject, IEnumerable<StopRecordItem>, IEnumerable
    {
        public StopRecord()
        {
        }


        private List<StopRecordItem> items = new List<StopRecordItem>();

        public double MaxSpeed
        {
            get
            {
                double[] arr = this.SpeedArray;
                if (arr.Length == 0)
                    return 0.0;

                return arr.Max();
            }
        }

        public double MinSpeed
        {
            get
            {
                double[] arr = this.SpeedArray;
                if (arr.Length == 0)
                    return 0.0;

                return arr.Min();
            }
        }

        /// <summary>
        /// 速度[m/min.]の配列
        /// </summary>
        public double[] SpeedArray
        {
            get
            {
                return items.Select(item => item.Speed).ToArray();
            }
        }
        
        /// <summary>
        /// 停止距離[m]の配列
        /// </summary>
        public double[] DistanceArray
        {
            get
            {
                return items.Select(item => item.Distance).ToArray();
            }
        }
        
        ///// <summary>
        ///// 停止位置情報を生成する。
        ///// </summary>
        ///// <param name="speedArr">速度の列</param>
        ///// <param name="distanceArr">距離の列</param>
        ///// <returns>停止位置情報</returns>
        //public static StopRecord Create(double[] speedArr, double[] distanceArr)
        //{
        //    if (speedArr == null || distanceArr == null)
        //        throw new ArgumentNullException();
        //    else if (speedArr.Length != distanceArr.Length)
        //        throw new InvalidOperationException();

        //    var newObj = new StopRecord();

        //    int count = speedArr.Length;
        //    for(int i = 0; i < count; i++)
        //    {
        //        newObj.Items.Add(new StopDistanceItem() { Speed = speedArr[i], Distance = distanceArr[i] });
        //    }

        //    return newObj;
        //}

        /// <summary>
        /// 追加する
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="distance"></param>
        public void Add(double speed, double distance)
        {
            this.items.Add(new StopRecordItem() { Speed = speed, Distance = distance });
        }

        /// <summary>
        /// 全ての情報を削除する。
        /// </summary>
        public void Clear()
        {
            this.items.Clear();
        }

        public IEnumerator<StopRecordItem> GetEnumerator()
        {
            return this.items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
    }

    public class StopRecordItem : GalaSoft.MvvmLight.ObservableObject
    {
        private double speed;

        /// <summary>
        /// 速度[m/min.]
        /// </summary>
        public double Speed        
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                RaisePropertyChanged(() => Speed);
            }
        }
        
        private double distance;

        /// <summary>
        /// 停止距離[m]
        /// </summary>
        public double Distance        
        {
            get
            {
                return distance;
            }
            set
            {
                distance = value;
                RaisePropertyChanged(() => Distance);
            }
        }
    }
}
