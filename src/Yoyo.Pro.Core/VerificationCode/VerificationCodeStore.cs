using System;
using System.DrawingCore;
using System.DrawingCore.Imaging;
using System.IO;
using System.Text;
using Abp;
using Yoyo.Pro.Extensions;

namespace Yoyo.Pro.VerificationCode
{
    /// <summary>
    /// 生成验证码，支持英文、英文加数字、数字及汉子
    /// </summary>
    public class VerificationCodeStore : IVerificationCodeStore
    {
        public MemoryStream Create(out string code, ValidateCodeType validateCodeType = ValidateCodeType.Number, int numberLength = 4)
        {
            switch (validateCodeType)
            {
                case ValidateCodeType.English:
                    code = GetRandomLetters(numberLength);
                    break;
                case ValidateCodeType.NumberAndLetter:
                    code = GetRandomNumsAndLetters(numberLength);
                    break;
                case ValidateCodeType.Hanzi:
                    code = GetRandomHanzis(numberLength);
                    break;
                default:
                    code = GetRandomNums(numberLength);
                    break;
            }


            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            var random = new Random();
            //验证码颜色集合  
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };

            //验证码字体集合
            string[] fonts = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };

           
            //定义图像的大小，生成图像的实例  
            Img = new Bitmap((int)code.Length * 18, 32);
            if (validateCodeType == ValidateCodeType.Hanzi)
            {
                Img = new Bitmap((int)code.Length * 22, 32);
            }
            g = Graphics.FromImage(Img);//从Img对象生成新的Graphics对象    

            g.Clear(Color.White);//背景设为白色  

            //在随机位置画背景点  
            for (var i = 0; i < 100; i++)
            {
                var x = random.Next(Img.Width);
                var y = random.Next(Img.Height);
                g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            }
            //验证码绘制在g中  
            for (var i = 0; i < code.Length; i++)
            {
                var cindex = random.Next(7);//随机颜色索引值  
                var findex = random.Next(5);//随机字体索引值  
                var f = new Font(fonts[findex], 15, FontStyle.Bold);//字体  
                Brush b = new SolidBrush(c[cindex]);//颜色  
                var ii = 6;
                if ((i + 1) % 2 == 0)//控制验证码不在同一高度  
                {
                    ii = 2;
                }
                g.DrawString(code.Substring(i, 1), f, b, 3 + (i * 12), ii);//绘制一个验证字符  
            }
            ms = new MemoryStream();//生成内存流对象  
            Img.Save(ms, ImageFormat.Png);//将此图像以Png图像文件的格式保存到流中  

            //回收资源  
            g.Dispose();
            Img.Dispose();
            return ms;
        }

        /// <summary>
        /// 获取数字验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码字符串</returns>
        private static string GetRandomNums(int length)
        {
            var ints = new int[length];
            for (var i = 0; i < length; i++)
            {
                ints[i] = RandomHelper.GetRandom(0, 9);
            }
            return ints.ExpandAndToString("");
        }


        /// <summary>  
        /// 获取英文的验证码  
        /// </summary>  
        /// <param name="length">验证码长度</param>  
        /// <returns>验证码字符串</returns>  
        private string GetRandomLetters(int length)
        {
            //验证码可以显示的字符集合  
            var Vchar = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z";

            var VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            var code = "";//产生的随机数  
            var temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            var rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (var i = 1; i < length + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                var t = rand.Next(VcArray.Length);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return GetRandomLetters(length);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }

        /// <summary>  
        /// 获取数字+英文的验证码  
        /// </summary>  
        /// <param name="length">验证码长度</param>  
        /// <returns>验证码字符串</returns>  
        private string GetRandomNumsAndLetters(int length)
        {
            //验证码可以显示的字符集合  
            var Vchar = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,P,P,Q,R,S,T,U,V,W,X,Y,Z";

            var VcArray = Vchar.Split(new Char[] { ',' });//拆分成数组   
            var code = "";//产生的随机数  
            var temp = -1;//记录上次随机数值，尽量避避免生产几个一样的随机数  

            var rand = new Random();
            //采用一个简单的算法以保证生成随机数的不同  
            for (var i = 1; i < length + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));//初始化随机类  
                }
                var t = rand.Next(61);//获取随机数  
                if (temp != -1 && temp == t)
                {
                    return GetRandomNumsAndLetters(length);//如果获取的随机数重复，则递归调用  
                }
                temp = t;//把本次产生的随机数记录起来  
                code += VcArray[t];//随机数的位数加一  
            }
            return code;
        }

        /// <summary>
        /// 获取汉字验证码
        /// </summary>
        /// <param name="length">验证码长度</param>
        /// <returns>验证码字符串</returns>
        private static string GetRandomHanzis(int length)
        {
            //汉字编码的组成元素，十六进制数
            var baseStrs = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f".Split(',');
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("GB2312");
            string result = null;

            //每循环一次产生一个含两个元素的十六进制字节数组，并放入bytes数组中
            //汉字由四个区位码组成，1、2位作为字节数组的第一个元素，3、4位作为第二个元素
            for (var i = 0; i < length; i++)
            {
                var index1 = RandomHelper.GetRandom(11, 14);
                var str1 = baseStrs[index1];

                var index2 = index1 == 13 ? RandomHelper.GetRandom(0, 7) : RandomHelper.GetRandom(0, 16);
                var str2 = baseStrs[index2];

                var index3 = RandomHelper.GetRandom(10, 16);
                var str3 = baseStrs[index3];

                var index4 = index3 == 10 ? RandomHelper.GetRandom(1, 16) : (index3 == 15 ? RandomHelper.GetRandom(0, 15) : RandomHelper.GetRandom(0, 16));
                var str4 = baseStrs[index4];

                //定义两个字节变量存储产生的随机汉字区位码
                var b1 = Convert.ToByte(str1 + str2, 16);
                var b2 = Convert.ToByte(str3 + str4, 16);
                byte[] bs = { b1, b2 };

                result += encoding.GetString(bs);
            }
            return result;
        }


    }
}
