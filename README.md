# ToStringDotNet

The library purpose is to save you from handrwiting `ToString()` method implementations for your objects.

Instead of:

```csharp
class MyClass
{
    public string MyProperty { get; set; }

    public void ToString()
    {
        var sb = new StringBuilder();
        sb.Append("{\"MyProperty\":\"");
        sb.Append(this.MyProperty);
        sb.Append("\"}");
        return sb.ToString();
    }
}
```

You can write:

```csharp
class MyClass
{
    [ToString]
    public string MyProperty { get; set; }

    public void ToString()
    {
        return ToStringFormatter.Format(this);
    }
}
```

## How to

### Print an object

```csharp
class MyClass
{
    [ToString]
    public string MyProperty1 { get; set; }

    [ToString]
    public int MyProperty2 { get; set; }
}

...

var obj = new MyObject{MyProperty1 = "p1", MyProperty2 = 1};
var str = ToStringFormatter.Format(obj);
Console.WriteLine(str);
//{"MyProperty1":"p1","MyProperty2":1}
```

### Reorder properties

```csharp
class MyClass
{
    [ToString(1)] //lower priority - goes last
    public string MyProperty1 { get; set; }

    [ToString(2)] //higher priority - goes first
    public int MyProperty2 { get; set; }
}

...

var obj = new MyObject{MyProperty1 = "p1", MyProperty2 = 1};
var str = ToStringFormatter.Format(obj);
Console.WriteLine(str);
//{"MyProperty2":1,"MyProperty1":"p2"}
```

### Exclude inherited properties

```csharp
class MyParentClass
{
    [ToString]
    public string MyProperty1 { get; set; }
}

[ToStringInheritance(ToStringInheritance.NotInherit)]
class MyChildClass: MyParentClass
{
    [ToString]
    public string MyProperty2 { get; set; }
}

...

var obj = new MyChildClass{MyProperty1 = "p1", MyProperty2 = "p2"};
var str = ToStringFormatter.Format(obj);
Console.WriteLine(str);
//{"MyProperty2":"p2"}
```

### More examples

For more examples see the [ToStringDotNet.Test](./ToStringDotNet.Test) project.