﻿using System.Collections.Generic;

namespace mlee.Core.Library.IpTools
{

    public class City
    {

        /**
         * @var Reader
         */
        private readonly Reader reader;

        public City(string name) {
            reader = new Reader(name);
        }

        /// <summary>
        /// 获取国家、省、市
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public string[] find(string addr, string language)  {
            return reader.find(addr, language);
        }

        public Dictionary<string, string> findMap(string addr, string language)  {
            var data = reader.find(addr, language);
            if (data == null) {
                return null;
            }
            var fields = reader.getSupportFields();
            var m = new Dictionary<string, string>();
            for (int i = 0, l = data.Length; i<l; i++) {
                m.Add(fields[i], data[i]);
            }

            return m;
        }

        public CityInfo findInfo(string addr, string language)  {

            var data = reader.find(addr, language);
            if (data == null) {
                return null;
            }

            return new CityInfo(data);
        }

        public bool isIPv4()
        {
            return (reader.getMeta().IPVersion & 0x01) == 0x01;
        }

        public bool isIPv6()
        {
            return (reader.getMeta().IPVersion & 0x02) == 0x02;
        }

        public string[] fields()
        {
            return reader.getSupportFields();
        }

        public int buildTime()
        {
            return reader.getBuildUTCTime();
        }
    }
}
