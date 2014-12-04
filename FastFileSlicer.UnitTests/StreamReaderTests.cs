using System;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace FastFileSlicer.UnitTests
{
    public class StreamReaderTests
    {
        public class BufferSize_4
        {
            private const int BufferSize = 4;
            private readonly static string TextToSearch = "abcdefgh";

            [TestFixture]
            public class FindChar
            {
                private StreamReader target;

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Index_0_For_a_Char()
                {
                    var actual = this.target.FindChar('a');
                    Assert.AreEqual(0, actual);
                }

                [Test]
                public void Returns_Index_1_For_b_Char()
                {
                    var actual = this.target.FindChar('b');
                    Assert.AreEqual(1, actual);
                }

                [Test]
                public void Returns_Index_Minus_1_For_i_Char()
                {
                    var actual = this.target.FindChar('i');
                    Assert.AreEqual(-1, actual);
                }
            }

            [TestFixture]
            public class FindString
            {
                private StreamReader target;

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Index_0_For_a_String()
                {
                    var actual = this.target.FindString("a");
                    CollectionAssert.AreEqual(new int[1], actual);
                }

                [Test]
                public void Returns_Index_1_For_b_String()
                {
                    var actual = this.target.FindString("b");
                    CollectionAssert.AreEqual(new int[1] { 1 }, actual);
                }

                [Test]
                public void Returns_Indexes_2_3_For_b_String()
                {
                    var actual = this.target.FindString("cd");
                    CollectionAssert.AreEqual(new int[2] { 2,3 }, actual);
                }

                [Test]
                public void Returns_Indexex_6_7_Minus1_For_ghi_String()
                {
                    var actual = this.target.FindString("ghi");
                    CollectionAssert.AreEqual(new int[3] { -1,0,0 }, actual);
                }
            }

            [TestFixture]
            public class ReadBytesToChar
            {
                private StreamReader target;
                private readonly static string TextToSearch = "abd abe\tabf afg fgh abc dce cba";

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Text_abd_abe_For_tab_Char()
                {
                    var actual = this.target.ReadBytesToChar('\t');
                    Assert.AreEqual("abd abe", Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }

                [Test]
                public void Returns_Text_abd_abe_tab_abf_afg_fgh_ab_For_c_Char()
                {
                    var actual = this.target.ReadBytesToChar('c');
                    Assert.AreEqual("abd abe\tabf afg fgh ab", Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }

                [Test]
                public void Returns_Text_abd_For_space_Char()
                {
                    var actual = this.target.ReadBytesToChar(' ');
                    Assert.AreEqual("abd", Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }
            }
        }

        public class BufferSize_1024
        {
            private const int BufferSize = 1024;
            private readonly static string TextToSearch = "abcdefgh";

            [TestFixture]
            public class FindChar
            {
                private StreamReader target;

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Index_0_For_a_Char()
                {
                    var actual = this.target.FindChar('a');
                    Assert.AreEqual(0, actual);
                }

                [Test]
                public void Returns_Index_1_For_b_Char()
                {
                    var actual = this.target.FindChar('b');
                    Assert.AreEqual(1, actual);
                }

                [Test]
                public void Returns_Index_7_For_h_Char()
                {
                    var actual = this.target.FindChar('h');
                    Assert.AreEqual(7, actual);
                }

                [Test]
                public void Returns_Index_Minus_1_For_i_Char()
                {
                    var actual = this.target.FindChar('i');
                    Assert.AreEqual(-1, actual);
                }
            }

            [TestFixture]
            public class FindString
            {
                private StreamReader target;

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Index_0_For_a_String()
                {
                    var actual = this.target.FindString("a");
                    CollectionAssert.AreEqual(new int[1] { 0 }, actual);
                }

                [Test]
                public void Returns_Index_1_For_b_String()
                {
                    var actual = this.target.FindString("b");
                    CollectionAssert.AreEqual(new int[1] { 1 }, actual);
                }

                [Test]
                public void Returns_Indexes_6_7_For_gh_String()
                {
                    var actual = this.target.FindString("gh");
                    CollectionAssert.AreEqual(new int[2] { 6,7 }, actual);
                }

                [Test]
                public void Returns_Index_Minus_1_For_ghi_String()
                {
                    var actual = this.target.FindString("ghi");
                    CollectionAssert.AreEqual(new int[3] { 6,7,-1 }, actual);
                }
            }

            [TestFixture]
            public class FindString_Similar_Chains
            {
                private StreamReader target;
                private readonly static string TextToSearch = "abd abe abf afg fgh abc dce cba";

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Indexes_20_21_22_For_abc_String()
                {
                    var actual = this.target.FindString("abc");
                    CollectionAssert.AreEqual(new int[3] { 20,21,22 }, actual);
                }

                [Test]
                public void Returns_Indexes_28_29_30_For_cba_String()
                {
                    var actual = this.target.FindString("cba");
                    CollectionAssert.AreEqual(new int[3] { 28,29,30 }, actual);
                }
            }

            [TestFixture]
            public class ReadBytesToChar
            {
                private StreamReader target;
                private readonly static string TextToSearch = "abd abe\tabf afg fgh abc dce cba";

                [SetUp]
                public void Before_Each_Test()
                {
                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(TextToSearch));
                    this.target = new StreamReader(memoryStream, BufferSize);
                }

                [TearDown]
                public void After_Each_Test()
                {
                    this.target.Dispose();
                }

                [Test]
                public void Returns_Text_abd_abe_For_tab_Char()
                {
                    var actual = this.target.ReadBytesToChar('\t');
                    Assert.AreEqual("abd abe", Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }

                [Test]
                public void Returns_Text_abd_For_space_Char()
                {
                    var actual = this.target.ReadBytesToChar(' ');
                    Assert.AreEqual("abd", Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }

                [Test]
                public void Returns_Whole_Text_For_LF_Char()
                {
                    var actual = this.target.ReadBytesToChar('\n');
                    Assert.AreEqual(TextToSearch, Encoding.UTF8.GetString(actual.Item3, actual.Item1, actual.Item2));
                }
            }
        }
    }
}

