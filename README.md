# Foundation

**Foundation** is an extension framework to **.NET 6**.

## Features

- Additional base types and buffer manipulations
- Additional collections and extensions to collections
- Additional types for serialization and extensions
- Reflection helpers and extensions

examples:

|framework|namespace|
|---------|---------|
|.NET| `System`|
|Foundation|`Foundation`|
|.NET| `System.Collections.Generic`|
|Foundation|`Foundation.Collections.Generic`|
|.NET| `System.IO`|
|Foundation|`Foundation.IO`|

The namespaces are based on Microsoft .NET namespaces. It is easy for a .NET developer to find a particular component because of the familiar namespace scheme.

## Namespaces

### Foundation

This corresponds to the .NET counterpart System.

Some examples:

|component |description|
|----------|-----------|
|Disposable|Can be used to dispose components when exiting a method.|
|Event     |Removes all subscribers on Dispose().|
|For       |For loop that returns a list.|
|Fused     |Acts like a fuse. If blown the value won't change any longer.|
|HashCode  |Can create hashcodes from a list of objects and hashcodes.|
|Id        |Entity identifier|
|Identifier|This identifier is ideal for rapid prototyping, if you do not want to commit to a specific type yet.
|Opt       |Optional that can be Some or None.|
|Period    |Period of time including duration.|
|Result    |Result that can be Ok or Error.|
|TimeDef   |time definition.|

- **Disposable**

  ```csharp
  
  // example

  void Func()
  {
     var disposable = new Disposable(() => collection.CollectionChanged -= OnChanged);

    // no need to call collection.CollectionChanged -= OnChanged on every exit.
     if(conditon) return;
    
     ...

     //you can't forget necessary cleanups.
     if(condition) return;
     
  } 
  ```

- **Event**
  ```csharp

  // example

  var executed1 = false;
  void func1()
  {
      executed1 = true;
  }

  var executed2 = false;
  void func2()
  {
      executed2 = true;
  }
  
  // leaving the scope unregisters all subscriptions automatically.
  using var sut = new Event<Action>();

  sut.Subscribe(func1);
  sut.Subscribe(func2);

  sut.Publish();

  Assert.IsTrue(executed1);
  Assert.IsTrue(executed2);

  //calling subscribe twice does ignore the second subscription.
  var disposable = sut.Subscribe(func1);

  // unsubsribe the delegate.
  disposable.Dispose();
  ```
- **For**
```csharp
//example 1

var value = 1;

//generates [1, 2, 3, 4, 5]
var values = For.Returns(() => value++)
                .Take(5)
                .ToArray();

//example 2

//generates [1, 2, 3, 4, 5]
var values = For.StartAt(() => 1).Returns(value => ++value)
                                 .TakeUntil(x => x == 5)
                                 .ToArray();

```
- **Fused**
  ```csharp

  //example 1

  // before
  int init = -1;
  int result = init;

  result = service1.Get();

  if(result == init)
     result = service2.Get();

  if(result == init)
     result = service3.Get();

  //after you don't need any if statement

  var fused = Fused.Value(-1).BlowIfChanged();
  
  Assert.IsFalse(fused.IsBlown);

  //value is not changed
  fused.Value = -1;
  Assert.IsFalse(fused.IsBlown);
  Assert.AreEqual(-1, fused.Value);

  fused.Value = 4;
  Assert.IsTrue(fused.IsBlown);
  Assert.AreEqual(4, fused.Value);

  fused.Value = 6;
  Assert.IsTrue(fused.IsBlown);
  Assert.AreEqual(4, fused.Value);
  ```
- **HashCode**
  
  ```csharp

  //examples
  
  var hashCode = HashCode.FromHashCode(hashcode1, hashcode2, ...);

  var builder = HashCode.Create();

  builder.AddObject(obj1, obj2, ...);
  builder.AddHashCode(hc1, hc2, ...);

  builder.AddObjects(objects);
  builder.AddHashCodes(hashcodes);

  var hashCode = builder.GetHashCode();

  ```

- **Id**
  ```csharp

  // examples

  var invoiceNumber = new Id<Invoice>("294-5220381-3931160");

  var deviceId = new Id<Device>(Guid.NewGuid());
  var sessionId = new Id<Session>(Guid.NewGuid());
  ```

- **Identifier**
  ```csharp

  // example
  
  var id = new Identifier(Guid.NewGuid());

  // a later change of the type of the value does not lead to changes of the rest

  // e.g. var id = new Identifier("294-5220381-3931160")

  var invoice = new Invoice<Identifier>(id);
  ```

- **Opt<T>**
  
  ```csharp

  // examples

  var maybe = Opt.Maybe(value);

  if(maybe.IsSome) ...

  if(maybe.TryGet(out int number)) ...
  
  var some = Opt.Some(value);
  var none = Opt.None<int>();
  
  //case distinction without if statement.
  var values = List<int>();
  var noneCounter = 0;
  maybe.Match(x => values.Add(x), () => noneCounter++);

  var str = maybe.Match(x => x.ToString(), () => "unknown");

  var some = maybe.Or(otherValue);
  var some = maybe.Or(() => new MyObject());


  ```

- **Period**
  
  ```csharp

  // examples

  var start = new DateTime(2019, 1, 1);
  var end = new DateTime(2022,12, 31);

  var period = Period.New(start, end);

  var duration = period.Duration;
  var days = period.Days();
  var hours = period.Hours();
  var minutes = period.Minutes();
  var months = period.Months();

  if(period.IsBetween(start, end)) ...

  var diff = period.SymmetricDifference(otherPeriod);

  var union = period.Union(otherPeriod);
  union = period.Union(startDate, endDate);

  var intersected = period1.Intersect(period2);

  var listOfintersected = period1.Intersect(new [] { period2, period3, period4 });

  var isOverlapping = period1.IsOverlapping(period2);

  var isWithin = period.IsWithin(new DateTime(2015, 6, 1)); // is the date within the period?

  var diff = period1.SymmetricDifference(period2);

  var merged = period1.Union(period2); // only merge when overlapping.
  ```

- **Result**
  
  ```csharp

  // examples

  var ok = Result.Ok(value);

  var result = Result.Error(new NotImplementedException());
  if(result.IsError) throw result.Error;
  
  ```
- **TimeDef**
  ```csharp

  //examples

  var day = new TimeDef.Day(2); // year-month-day
  
  var duration = TimeDef.DateTimeSpan(from, to);
  
  var days = new TimeDef.Days(3); // number of days
  
  var hour = new TimeDef.Hour(8); // 08:00

  var minutes = new TimeDef.Minutes(30);

  var months = new TimeDef.Month(Month.Apr, Month.Jul);

  var weekday = new TimeDef.Weekday(DayOfWeek.Monday, DayOfWeek.Wednesday);
  var notMondayAndWednesday = new TimeDef.Not(weekday);

  var day2 = new TimeDef.Day(2);
  var day3 = new TimeDef.Day(3);
  var day2OrDay3 = new TimeDef.Or(day2, day3);

  var td1 = new TimeDef.And(
               new TimeDef.And(
                  new TimeDef.Year(2015),
                  new TimeDef.Month(9)),
               new TimeDef.Day(12));

   var td2 = new TimeDef.And(
               new TimeDef.And(
                  new TimeDef.Year(2015),
                  new TimeDef.Month(11)),
               new TimeDef.Day(1));

  td1.Equals(td2);
  TimeDef.Difference(td1, td2);
  TimeDef.Union(td1, td2);
  ```

## Foundation.Collections.Generic

This corresponds to the .NET counterpart System.Collections.Generic.

### EnumerableExtensions

Some examples:

|method |description        |
|---------------------------|-----------|
|Adjacent                   |Calls action on all adjacent elements.|
|AfterEach                  |Is called after iterating each element except the last one.|
|AllEqual                   |Return true when all elements are equal.|
|AtLeast                    |Returns at least a number of elements. If the number of elements is smaller, an empty enumerable is returned.|
|AverageMedian              |Returns the median of all values returned by the converter.|
|AverageMedianValues        |Returns the real values instead of a division of the median values.|
|Contains                   |Checks if lhs contains at least one element of rhs.|
|CartesianProduct           |Returns a cartesian product of two lists.|
|Contains                   |Checks if lhs contains at least one element of rhs.
|CyclicEnumerate            |Creates an endless list of items.|
|Difference                 |Returns the symmetric difference of two lists.|
|Duplicates                 |Returns duplicate items of a list. If there are e.g. three of an item, 2 will returned.|
|Enumerate                  |Enumerates items. Returns tuples of (item, counter).|
|FilterMap                  |Filters and transform items. It returns only Opt.Some values.|
|FindUntil                  |Searches items until all predicates matched exactly one time.|
|FirstAsOpt                 |Returns first item as Opt. If the list is empty None is returned.|
|Ignore                     |Ignores items when predicate returns true.|
|IsEqualTo                  |Returns true, if all elements of items appear in the other list, the number of items and the occurrence are same.|
|KCombinations              |Returns a list of k-combinations without repetition.|
|KCombinationsWithRepetition|Returns a list of k-combinations with repetitions.|
|Match                      |Returns all matching items of both lists as a tuple of two lists.|
|MatchWithOccurrencies      |Returns matching items of both lists with their occurrencies.|
|MinMax                     |Returns the min and max value.|
|MostFrequent               |returns the elements that occure most frequently.|
|OnFirst                    |Calls action on first item.|
|OnLast                     |Calls action on last item.|
|Partition                  |Partitions items into two lists. If predicate is true the item is added to matching otherwise to notMatching.|
|Permutations               |Creates permutations of a list.|
|ToBreakable                |Makes the enumerable interruptible. This can be used for nested foreach loops.|

- **Adjacent**

  ```csharp
  
  // example

  var numbers = Enumerable.Range(0, 5);
  var tuples = new List<(int, int)>();

  foreach (var _ in numbers.Adjacent((prev, curr) => tuples.Add((prev, curr))))
  {
  }

  Assert.AreEqual(4, tuples.Count);

  var it = tuples.GetEnumerator();

  Assert.IsTrue(it.MoveNext());
  Assert.AreEqual((0, 1), it.Current);

  Assert.IsTrue(it.MoveNext());
  Assert.AreEqual((1, 2), it.Current);

  Assert.IsTrue(it.MoveNext());
  Assert.AreEqual((2, 3), it.Current);

  Assert.IsTrue(it.MoveNext());
  Assert.AreEqual((3, 4), it.Current); 
  ```

- **AfterEach**
  
  ```csharp

  // example

  var items = new List<string> { "1", "2", "3" };
  var sb = new StringBuilder();

  foreach (var item in items.AfterEach(() => sb.Append(',')))
  {
     sb.Append(item);
  }

  var actual = sb.ToString();
  Assert.AreEqual("1,2,3", actual);
  ```

- **AllEqual**
```csharp

//example

var numbers = Enumerable.Repeat(1, 10);

//returns true
var eq = numbers.AllEqual();

```

- **AtLeast**
  
  ```csharp

  // examples

  var items = new List<string> { "1", "2", "3" }.ToArray();
  {
     var actual = items.AtLeast(4).ToArray();
     Assert.AreEqual(0, actual.Length);
  }
  {
     var actual = items.AtLeast(2).ToArray();
     Assert.AreEqual(3, actual.Length);
  }
  {
     var actual = items.AtLeast(3).ToArray();
     Assert.AreEqual(3, actual.Length);
  }
  ```

- **AverageMedian**

  ```csharp

  //odd number of elements
  {
     var numbers = Enumerable.Range(1, 7);

     var median = numbers.AverageMedian();

     Assert.AreEqual(4, median);
  }
  
  //even number of elements
  {
     var numbers = Enumerable.Range(1, 8);

     var median = numbers.AverageMedian();

     Assert.AreEqual(4.5, median);
  }
  ```

- **AverageMedianValues**

  ```csharp

  //examples

  {
     var numbers = Enumerable.Range(1, 7);

     var (opt1, opt2) = numbers.AverageMedianPosition();

     Assert.IsFalse(opt2.IsSome);
     Assert.AreEqual(4, opt1.ValueOrThrow());
  }
  {
     var numbers = Enumerable.Range(1, 8);

     var (opt1, opt2) = numbers.AverageMedianPosition();

     Assert.IsTrue(opt2.IsSome);
     Assert.AreEqual(4, opt1.ValueOrThrow());
     Assert.AreEqual(5, opt2.ValueOrThrow());
  }
  {
     var items = Enumerable.Range(1, 7).Select(x => x.ToString());

     var (opt1, opt2) = items.AverageMedianPosition();

     Assert.IsFalse(opt2.IsSome);
     Assert.AreEqual("4", opt1.ValueOrThrow());
  }
  {
     var items = Enumerable.Range(1, 8).Select(x => x.ToString());

     var (opt1, opt2) = items.AverageMedianPosition();

     Assert.IsTrue(opt2.IsSome);
     Assert.AreEqual("4", opt1.ValueOrThrow());
     Assert.AreEqual("5", opt2.ValueOrThrow());
  }
  ```

- **Contains**
  ```csharp
  var items1 = Enumerable.Range(0, 9);
  var items2 = Enumerable.Range(0, 9).Where(i => (i % 2) == 0);

  var result = items.Contains(items2);
  ```

- **CyclicEnumerate**

  ```csharp

  //example 1

  {
     var items = new List<string> { "A", "B", "C" };

     var e = items.CycleEnumerate().GetEnumerator();
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("A", e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("B", e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("C", e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("A", e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("B", e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual("C", e.Current);
  }

  // example 2
  {
     var items = new List<string> { "A", "B", "C", "D", "E" };

     var enumerated = items.CycleEnumerate(1, 2).ToList();

     var e = enumerated.GetEnumerator();
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual((1, "A"), e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual((2, "B"), e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual((1, "C"), e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual((2, "D"), e.Current);
     
     Assert.IsTrue(e.MoveNext());
     Assert.AreEqual((1, "E"), e.Current);
     
     Assert.IsFalse(e.MoveNext());
  }
  ```

- **Difference**

  ```csharp

  
  //example 1
  {
     var items1 = Enumerable.Range(0, 10);
     var items2 = Enumerable.Range(10, 10);

     // return all items because lists are completely different
     var diff = items1.Difference(items2).ToArray();

     Assert.AreEqual(20, diff.Length);
  }

  //example 2
  {
     var items1 = new List<int> { 1, 2, 3, 4, 5 };
     var items2 = new List<int> { 2, 4, 6 };

     // returns the symmetric difference
     var diff = items1.Difference(items2).ToArray();

     Assert.AreEqual(4, diff.Length);
     CollectionAssert.AreEqual(new[] { 1, 3, 5, 6 }, diff);
  }
  ```

- **Duplicates**

  ```csharp
  
  // example 1
  {
     var items = new List<int> { 1, 2, 3, 4, 5, 2, 4, 2 };
     var result = items.Duplicates().ToArra(;

     // 3 duplicates found
     Assert.AreEqual(3, result.Cout;

     // 2 appears 3 times in the list, two of them are duplicates
     Assert.AreEqual(2, result[0]);
     Assert.AreEqual(2, result[];

     // 4 appears 2 times in the list, one of them is a duplicate.
     Assert.AreEqual(4, result[2]);
  }
  
  // example 2
  {
     var items = new List<int> { 1, 2, 3, 4, 5, 2, 4, 2 };

     // If distinct is true, only one example of every duplicate item is     returned. 
     var result = items.Duplicates(distinct: true).ToArray();

     Assert.AreEqual(2, result.Count);
     Assert.AreEqual(2, result[0]);
     Assert.AreEqual(4, result[1]);
  }
  ```
- **Enumerate**

  ```csharp

  // example 1
  {
     var items = new[] { "one", "two", "three" };

     var enumerated = items.Enumerate().ToArray();

     Assert.AreEqual(("one",   0), enumerated[0]);
     Assert.AreEqual(("two",   1), enumerated[1]);
     Assert.AreEqual(("three", 2), enumerated[2]);
  }
  // example 2
  {
     var items = new[] { "one", "two", "three" };
     var i = 10;

     var enumerated = items.Enumerate(item => i++).ToArray();

     Assert.AreEqual(("one",   10), enumerated[0]);
     Assert.AreEqual(("two",   11), enumerated[1]);
     Assert.AreEqual(("three", 12), enumerated[2]);
  }
  
  // example 3
  {
     var items = Enumerable.Range(1, 8);
    
     // { (1, 10), (2, 11), (3, 12), (4, 10), (5, 11), (6, 12), (7, 10), (8, 11) }
     var enumerated = items.Enumerate(10..12).ToArray();
    
     //with negative counter
     // { (1, -1), (2, 0), (3, 1), (4, -1), (5, 0), (6, 1), (7, -1), (8, 0) }
     var enumerated = items.Enumerate(MinMax.New(-1, 1)).ToArray();
  }
 
  // example 4
  {
     var items1 = Enumerable.Range(0, 9).ToArray();
     
     var enumerated = items1.Enumerate(n => n * 2).ToArray();
     
     Assert.AreEqual(items1.Length, enumerated.Length);
     
     foreach (var (item, counter) in enumerated)
         Assert.AreEqual(item * 2, counter);
  }
  
  ```

- **FindUntil**

  ```csharp

  // example

  var numbers = new [] { 1, 2, 3, 2, 4, 4, 5, 6, 7 };
  var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4, n => n == 6 };

  // if a predicate is fulfilled it is no longer used
  var foundNumbers = numbers.FindUntil(predicates).ToArray();

  Assert.AreEqual(3, foundNumbers.Length);
  Assert.AreEqual(2, foundNumbers[0]);
  Assert.AreEqual(4, foundNumbers[1]);
  Assert.AreEqual(6, foundNumbers[2]);
  ```

- **FirstAsOpt**
```csharp
  var items = Enumerable.Empty<int>();
  
  // you don't know if list is empty or contains 0.
  var item = items.FirstOrDefault();

  var optional = items.FirstAsOpt();

  // item exists.
  if(optional.IsSome)...

  // item does not exist
  if(optional.IsNone)...

```

- **Ignore**

  ```csharp

  // example 1
  {
     var numbers = Enumerable.Range(0, 10);
     var filtered = numbers.Ignore(new[] { 1, 3, 5, 7, 9 }).ToArray();
     
     Assert.AreEqual(5, filtered.Length);

     Assert.AreEqual(0, filtered[0]);
     Assert.AreEqual(2, filtered[1]);
     Assert.AreEqual(4, filtered[2]);
     Assert.AreEqual(6, filtered[3]);
     Assert.AreEqual(8, filtered[4]);
  }

  // example 2
  {
     var numbers = Enumerable.Range(0, 10);

     var filtered = numbers.Ignore(n => n % 2 == 0).ToArray();

     Assert.AreEqual(5, filtered.Length);
     
     Assert.AreEqual(1, filtered[0]);
     Assert.AreEqual(3, filtered[1]);
     Assert.AreEqual(5, filtered[2]);
     Assert.AreEqual(7, filtered[3]);
     Assert.AreEqual(9, filtered[4]);
  }
  ```
- **IsEqualTo**
  ```csharp

  // example 1
  {
    var items1 = new[] { 1, 2, 3, 2 };
    var items2 = new[] { 2, 3, 2, 1 };

    Assert.IsTrue(items1.IsEqualTo(items2));
    Assert.IsTrue(items2.IsEqualTo(items1));
  }

  // example 2
  {
     var keyValues1 = new Dictionary<string, object>
     {
       { "one",   1 },
       { "two",   2 },
       { "three", 3 }
     };

     var keyValues1 = new Dictionary<string, object>
     {
       { "three", 3 }
       { "one",   1 },
       { "two",   2 },
     };

     // dictionary Equals
     Assert.IsFalse(items1.Equals(items2));

     //IsEqualTo
     Assert.IsTrue(items1.IsEqualTo(items2));
  }
  ```

- **KCombinations**
```csharp
  // example
  {
     var numbers = Enumerable.Range(1, 3);

     //k-combinations without repetitions
     var kCombinations = numbers.KCombinations(2).ToArray();

     Assert.AreEqual(3, kCombinations.Length);
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 1, 2 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 1, 3 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 2, 3 })));
  }
```

- **KCombinationsWithRepetition**
```csharp
  // example
  {
     var numbers = Enumerable.Range(1, 3);

     //k-combinations without repetitions
     var kCombinations = numbers.KCombinationsWithRepetition(2).ToArray();

     Assert.AreEqual(6, kCombinations.Length);
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 1, 1 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 1, 2 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 1, 3 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 2, 2 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 2, 3 })));
     Assert.IsTrue(kCombinations.Any(g => g.IsEqualTo(new[] { 3, 3 })));
  }
```

- **Match**
  ```csharp

  //example

  var dates1 = new List<DateTime>
  {
     new DateTime(2017, 4, 13),
     new DateTime(2017, 5,  2),
     new DateTime(2017, 9,  3),
     new DateTime(2018, 7,  1)
  };

  var dates2 = new List<DateTime>
  {
     new DateTime(2015, 4, 29),
     new DateTime(2019, 2,  5),
     new DateTime(2019, 6,  1),
     new DateTime(2020, 4,  1)
  };

  // lhs contains matching items from dates1.
  // rhs contains matching items from dates2.
  var (lhs, rhs) = dates1.Match(dates2, dt => new { dt.Day, dt.Month });

  var lhsFound = lhs.Single();
  Assert.AreEqual(new DateTime(2017, 4, 13), lhsFound);

  var rhsFound = rhs.ToArray();
  Assert.AreEqual(2, rhsFound.Length);

  Assert.AreEqual(new DateTime(2015, 4, 29), rhsFound[0]);
  Assert.AreEqual(new DateTime(2020, 4,  1), rhsFound[1]);
  ```

- **MinMax**

  ```csharp  

  // example  

  var numbers = new[] { 1, 2, 2, 2, 5, 3, 3, 3, 3, 4 };
  
  //returns min and max of numbers
  var actual = numbers.MinMax();  

  var expected = MinMax.New(1, 5);
  Assert.AreEqual(expected, actual);

  ```

- **MostFrequent**
  ```csharp

  // example

  var numbers = new[] { 1, 2, 2, 3, 3, 3, 3, 4 };

  var (mostFrequent, count) = numbers.MostFrequent(x => x);

  var items = mostFrequent.ToArray();
  Assert.AreEqual(1, items.Length);
  Assert.AreEqual(3, items[0]);     // 3 occurs most often
  Assert.AreEqual(4, count);        // occurrs 4 times
  ```

- **OnFirst, OnLast**

  ```csharp

  // example

  var numbers = Enumerable.Range(1, 5);
  var sb = new StringBuilder();

  foreach(var number in numbers.OnFirst(() => sb.Append("("))
                               .AfterEach(() => sb.Append(", "))
                               .OnLast(() => sb.Append(")"))
  {
      sb.Append(number);
  }
  
  var str = sb.ToString();

  Assert.AreEqual("(1, 2, 3, 4, 5)", str);
  ```

- **Partition**
  ```csharp

  // example

  var numbers = Enumerable.Range(1, 10);

  var (matching, notMatching) = numbers.Partition(x => x % 2 == 0);

  var even = matching.ToArray();
  var odd = notMatching.ToArray();

  Assert.AreEqual(5, even.Length);
  Assert.AreEqual(5, odd.Length);

  Assert.Contains(2,  even);
  Assert.Contains(4,  even);
  Assert.Contains(6,  even);
  Assert.Contains(8,  even);
  Assert.Contains(10, even);

  Assert.Contains(1, odd);
  Assert.Contains(3, odd);
  Assert.Contains(5, odd);
  Assert.Contains(7, odd);
  Assert.Contains(9, odd);
  ```

- **Permutations**

  ```csharp
   
  // example 1
  {
     // this example include repetitions

     var numbers = new[] { 1, 2, 3 };

     var permutations = numbers.Permutations(2).ToArray();

     Assert.AreEqual(9, permutations.Length);

     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 1, 1 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 1, 2 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 1, 3 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 2, 1 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 2, 2 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 2, 3 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 3, 1 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 3, 2 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 3, 3 })));
  }
  
  // example 2
  {
     // this example does not include repetitions

     var numbers = new[] { 1, 2, 3 };

     var permutations = numbers.Permutations(2, false).ToArray();

     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 1, 2 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 1, 3 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 2, 1 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 2, 3 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 3, 1 })));
     Assert.IsTrue(permutations.Any(g => g.IsEqualTo(new[] { 3, 2 })));
  }
  ```

- **ToBreakable**

  ```csharp

  // example 1
  {
     var items1 = Enumerable.Range(1, 3);
     var items2 = Enumerable.Range(1, 3);
     var items3 = Enumerable.Range(1, 3);

     var i1 = 0;
     var i2 = 0;
     var i3 = 0;

     foreach (var item1 in items1)
     {
        var stop = ObservableValue.Create(false);

        i1++;
        foreach (var item2 in items2.ToBreakable(ref stop))
        {
           i2++;
           foreach (var item3 in items3.ToBreakable(ref stop))
           {
              i3++;
              if (item3 == 2) stop.Value = true;
           }
        }
     }

     Assert.AreEqual(3, i1);
     Assert.AreEqual(3, i2);
     Assert.AreEqual(6, i3);
  }

  // example 2
  {
      var items1 = Enumerable.Range(0, 3);
      var items2 = Enumerable.Range(0, 3);
      var items3 = Enumerable.Range(0, 3);

      var stop = ObservableValue.Create(false);
      var stopAll = ObservableValue.Create(false);

      foreach (var item1 in items1.ToBreakable(ref stopAll))
      {
          foreach (var item2 in items2.ToBreakable(ref stop)
                                      .ToBreakable(ref stopAll))
          {
              foreach (var item3 in items3.ToBreakable(ref stop)
                                          .ToBreakable(ref stopAll))
              {
                  if (item1 == 0 && item3 == 1)
                      stop.Value = true;

                  if (item2 == 1)
                      stopAll.Value = true;
              }
          }
      }
  }
  ```
