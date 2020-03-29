using System;
using System.Text.RegularExpressions;

namespace FakeXiecheng.API.Helpers
{
    public enum OperatorType
    {
        lessThan,
        largerThan,
        equal,
    }

    public class TouristRouteFilterParameters
    {
        /**
         * pagination
         */
        const int maxPageSize = 20;
        public int _pageNumber = 1;
        public int PageNumber {
            get { return _pageNumber; }
            set {
                if( value >= 1)
                {
                    _pageNumber = value;
                }
            }
        }
        private int _pageSize = 10;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (value >= 1)
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }
        }

        /**
         * 关键词搜索
         */
        public string Keyword { get; set; }

        /**
         * Filter: rating
         * 接受 url rating 参数
         * 配置 ratingVlaue 和 ratingOperator
         */
        private OperatorType _ratingOperator;
        public OperatorType RatingOperator { get { return _ratingOperator; } }
        private int _ratingVlaue;
        public int RatingValue { get { return _ratingVlaue; } }
        private string _rating; 
        public string Rating
        {
            get { return _rating; }
            set
            {
                Regex regex = new Regex(@"([A-Za-z0-9\-]+)(\d+)");
                Match match = regex.Match(value);
                if (match.Success)
                {
                    var ratingType = match.Groups[1].Value;
                    var ratingValue = match.Groups[2].Value;
                    try
                    {
                        _ratingVlaue = Int32.Parse(ratingValue);
                        if (ratingType.Equals("lessThan"))
                        {
                            _ratingOperator = OperatorType.lessThan;
                        }
                        else if (ratingType.Equals("largerThan"))
                        {
                            _ratingOperator = OperatorType.largerThan;
                        }
                        else
                        {
                            _ratingOperator = OperatorType.equal;
                        }
                    } catch
                    {
                        // 什么都不用做
                    }
                }
                _rating = value;
            }
        }

        /**
         * 排序
         */
        public string OrderBy { get; set; } = "Title";
    }
}
