# Foundation

**Foundation** is an extension framework to .NET. It includes a lot of functionalities needed for the daily work of a C# developer.

The namespaces are based on Microsoft .NET namespaces.

example:

|framework|namespace|
|---------|---------|
|.NET| ```System.Collections.Generic```|
|Foundation|```Foundation.Collections.Generic```|

It is easy for a .NET developer to find a particular component because of the familiar namespace scheme.

## Namespaces

### Foundation

This corresponds to the .NET counterpart System.

Some examples:

|component |description|
|----------|-----------|
|Disposable|Can be used to dispose components when exiting a method.|
|Event     |Removes all subscribers on Dispose().|
|HashCode  | Can create hashcodes from a list of objects and hashcodes.|
|Opt       |Optional that can be Some or None.|
|Period    |Period of time including duration.|
|Result    |Result that can be Ok or Error.|

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

- **Opt<T>**
  
  ```csharp

  // examples

  var maybe = Opt.Maybe(value);

  if(maybe.IsSome) ...

  if(maybe.IsSome(out int number)) ...

  var none = Opt.None<int>();

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

  ```

- **Result**
  
  ```csharp

  // examples

  var ok = Result.Ok(value);

  var result = Result.Error(new NotImplementedException());
  if(result.IsError) throw result.Error;
  
  ```

## Foundation.Collections.Generic

This corresponds to the .NET counterpart System.Collections.Generic.

### EnumerableExtensions

Some examples:

|method |description  |
|---------------------|-----------|
|AfterEveryElement    |Is called after iterating each element except the last one.|
|AtLeast              |Returns at least a number of elements. If the number of elements is smaller, an empty enumerable is returned.|
|AverageMedian        |Returns the median of all values returned by the converter.|
|AverageMedianValues  |Returns the real values instead of a division of the median values.|
|CartesianProduct     |Returns a cartesian product of two lists.|
|CyclicEnumerate      |Creates an endless list of items.|
|Difference           |Returns the symmetric difference of two lists.|
|Duplicates           |Returns duplicate items of a list. If there are e.g. three of an item, 2 will returned.|
|Enumerate            |Enumerates items. Returns tuples (item, counter).|
|FilterMap            |Filters and transform items. It returns only Opt.Some values.|
|FindUntil            |Searches items until all predicates matched exactly one time.|
|Ignore               |Ignores items when predicate returns true.|
|IsEqualTo            |Returns true, if all elements of items appear in the other list, the number of items and the occurrence are same.|
|Match                |Returns all matching items of both lists as a tuple of two lists.|
|MatchWithOccurrencies|Returns matching items of both lists with their occurrencies.|
|MinMax               |Returns the min and max value.|
|MostFrequent         |returns the elements that occure most frequently.|
|OnAdjacentElements   |Calls action on all adjacent elements.|
|OnFirst              |Calls action on first item.|
|OnLast               |Calls action on last item.|
|Partition            |Partitions items into two lists. If predicate is true the item is added to matching otherwise to notMatching.|
|Permutations         |Creates permutations of a list.|
|ToBreakable          |Makes the enumerable interruptible. This can be used for nested foreach loops.|

- **AfterEveryElement**
  
  ```csharp

  // example

  var items = new List<string> { "1", "2", "3" };
  var sb = new StringBuilder();

  foreach (var item in items.AfterEveryElement(() => sb.Append(',')))
  {
     sb.Append(item);
  }

  var actual = sb.ToString();
  Assert.AreEqual("1,2,3", actual);
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

  //examples
  
  {
     var items1 = Enumerable.Range(0, 10);
     var items2 = Enumerable.Range(10, 10);

     // return all items because lists are completely different
     var diff = items1.Difference(items2).ToArray();

     Assert.AreEqual(20, diff.Length);
  }
  {
     var items1 = new List<int> { 1, 2, 3, 4, 5 };
     var items2 = new List<int> { 2, 4, 6 };

     // return items of both lists that don't match
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

  // example

  var items = new[] { "one", "two", "three" };
  var i = 0;

  var enumerated = items.Enumerate(item => i++).ToArray();

  Assert.AreEqual(("one",   0), enumerated[0]);
  Assert.AreEqual(("two",   1), enumerated[1]);
  Assert.AreEqual(("three", 2), enumerated[2]);
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

  // example

  var items1 = new[] { 1, 2, 3, 2 };
  var items2 = new[] { 2, 3, 2, 1 };

  Assert.IsTrue(items1.IsEqualTo(items2));
  Assert.IsTrue(items2.IsEqualTo(items1));
  ```

- **Match**
  ```csharp

  //example

  var dates1 = new List<DateTime>
  {
	 new DateTime(2017, 4, 1),
	 new DateTime(2017, 5, 2),
	 new DateTime(2017, 9, 3),
	 new DateTime(2018, 7, 1)
  };

  var dates2 = new List<DateTime>
  {
	 new DateTime(2019, 2, 5),
	 new DateTime(2019, 6, 1),
	 new DateTime(2020, 4, 1)
  };

  var (lhs, rhs) = dates1.Match(dates2, dt => new { dt.Day, dt.Month });

  var lhsFound = lhs.Single();
  var rhsFound = rhs.Single();

  Assert.AreEqual(new DateTime(2017, 4, 1), lhsFound);
  Assert.AreEqual(new DateTime(2020, 4, 1), rhsFound);
  ```

- **MinMax**

  ```csharp  

  // example  

  var numbers = new[] { 1, 2, 2, 2, 5, 3, 3, 3, 3, 4 };

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
- **OnAdjacentElements**

  ```csharp
  
  // example

  var numbers = Enumerable.Range(0, 5);
  var tuples = new List<(int, int)>();

  foreach (var _ in numbers.OnAdjacentElements((prev, curr) => tuples.Add((prev, curr))))
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

- **OnFirst, OnLast**

  ```csharp

  // example

  var numbers = Enumerable.Range(1, 5);
  var sb = new StringBuilder();

  foreach(var number in numbers.OnFirst(() => sb.Append("("))
                               .AfterEveryElement(() => sb.Append(", "))
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

  var (matching, notMatching) numbers.Partition(x => x % 2 == 0);

  var matchingNumbers = matching.ToArray();
  var notMatchingNumbers = notMatching.ToArray();

  Assert.AreEqual(5, matchingNumbers.Length);
  Assert.AreEqual(5, notMatchingNumbers.Length);

  Assert.Contains(2,  matchingNumbers);
  Assert.Contains(4,  matchingNumbers);
  Assert.Contains(6,  matchingNumbers);
  Assert.Contains(8,  matchingNumbers);
  Assert.Contains(10, matchingNumbers);

  Assert.Contains(1, notMatchingNumbers);
  Assert.Contains(3, notMatchingNumbers);
  Assert.Contains(5, notMatchingNumbers);
  Assert.Contains(7, notMatchingNumbers);
  Assert.Contains(9, notMatchingNumbers);
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
