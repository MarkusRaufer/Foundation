//using BenchmarkDotNet.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Foundation.Benchmark;

//[MemoryDiagnoser]
//public class OptionBenchmarks
//{
//    private readonly DateTime _dateTime = DateTime.Now;
//    private readonly string _str = "This is a test string";

//    [Benchmark]
//    public Option<DateTime> OptionMaybeNoneDateTime()
//    {
//        return Option.Maybe<DateTime>(default);
//    }

//    [Benchmark]
//    public Opt<DateTime> OptMaybeNoneDateTime()
//    {
//        return Opt.Maybe<DateTime>(default);
//    }

//    [Benchmark]
//    public Option<string> OptionMaybeNoneString()
//    {
//        return Option.Maybe<string>(default);
//    }

//    [Benchmark]
//    public Opt<string> OptMaybeNoneString()
//    {
//        return Opt.Maybe<string>(default);
//    }

//    [Benchmark]
//    public Option<DateTime> OptionMaybeSomeDateTime()
//    {
//        return Option.Maybe(_dateTime);
//    }

//    [Benchmark]
//    public Opt<DateTime> OptMaybeSomeDateTime()
//    {
//        return Opt.Maybe(_dateTime);
//    }

//    [Benchmark]
//    public Option<string> OptionMaybeSomeString()
//    {
//        return Option.Maybe(_str);
//    }

//    [Benchmark]
//    public Opt<string> OptMaybeSomeString()
//    {
//        return Opt.Maybe(_str);
//    }

//    [Benchmark]
//    public Option<string> OptionNoneString()
//    {
//        return Option.None<string>();
//    }

//    [Benchmark]
//    public Opt<string> OptNoneString()
//    {
//        return Opt.None<string>();
//    }

//    [Benchmark]
//    public Opt<string> OptNoneString_Ctor()
//    {
//        return new();
//    }

//    [Benchmark]
//    public Option<DateTime> OptionSomeDateTime()
//    {
//        return Option.Some(_dateTime);
//    }

//    [Benchmark]
//    public Opt<DateTime> OptSomeDateTime()
//    {
//        return Opt.Some(_dateTime);
//    }

//    [Benchmark]
//    public Opt<DateTime> OptSomeDateTime_Ctor()
//    {
//        return new(_dateTime);
//    }

//    [Benchmark]
//    public Option<string> OptionSomeString()
//    {
//        return Option.Some(_str);
//    }

//    [Benchmark]
//    public Opt<string> OptSomeString()
//    {
//        return Opt.Some(_str);
//    }

//    [Benchmark]
//    public Opt<string> OptSomeString_Ctor()
//    {
//        return new(_str);
//    }
//}
