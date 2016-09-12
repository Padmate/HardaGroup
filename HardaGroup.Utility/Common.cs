using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Utility
{
    public static class Common
    {
        
        #region 图片类型

        public const string Image_HomeBG = "home";
        public const string Image_AboutBG = "about";
        public const string Image_AdvancedManufacturingBG = "advancedmanufacturing";
        public const string Image_SolutionsBG_RD = "solutions-research-development";
        public const string Image_SolutionsBG_PM = "solutions-parts-modules";
        public const string Image_VentureSupportBG = "venturesupport";
        public const string Image_PressBG = "press";
        public const string Image_JoinUsBG = "joinus";
        public const string Image_ContactUsBG = "contactus";

        public const string News_Thumbnails = "news-picture";
        public const string Module_Thumbnails = "module-picture";

        public static Dictionary<string, string> Dic_ImageType = new Dictionary<string, string>(){
            {Image_HomeBG,"首页背景图片"},
            {Image_AboutBG,"关于华尔达背景图片"},
            {Image_AdvancedManufacturingBG,"智能制造背景图片"},
            {Image_SolutionsBG_RD,"解决方案-研发背景图片"},
            {Image_SolutionsBG_PM,"解决方案-组件产品背景图片"},
            {Image_VentureSupportBG,"创新创业背景图片"},
            {Image_PressBG,"媒体资讯背景图片"},
            {Image_JoinUsBG,"加入华尔达背景图片"},
            {Image_ContactUsBG,"联系我们背景图片"},
            {News_Thumbnails,"新闻缩略图"},
            {Module_Thumbnails,"模块缩略图"}

        };
        #endregion

        #region 国际化语言

        public const string Globalization_Chinese = "zh-cn";
        public const string Globalization_English = "en-us";

        public static Dictionary<string, string> Dic_Globalization = new Dictionary<string, string>(){
            {Globalization_Chinese,"中文"},
            {Globalization_English,"英文"}

        };
        #endregion

        #region ModuleType 模块类型
        /// <summary>
        /// 智能制造简介
        /// </summary>
        public const string ModuleType_Manufacturing_Introduction = "AdvancedManfacturingIntroduction";

        /// <summary>
        /// 智能制造服务范围
        /// </summary>
        public const string ModuleType_Manufacturing_ServiceScope = "AdvancedManfacturingServiceScope";

        /// <summary>
        /// 智能制造服务特色
        /// </summary>
        public const string ModuleType_Manufacturing_ServiceFeatures = "AdvancedManfacturingServiceFeatures";


        #endregion

    }
}
