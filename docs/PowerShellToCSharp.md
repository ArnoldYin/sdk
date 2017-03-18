# Language Conversion Tests
### This file was generated by tests that were run on 3/18/2017 5:22:18 PM.
# Convert PowerShell to CSharp
## While loop with break
### Source: PowerShell
```powershell
function Method
{
	while($true)
	{
		break
	}
}
```
### Target: CSharp
```csharp
void Method()
{
	while (true)
	{
		break;
	}
}
```

# Convert PowerShell to CSharp
## Common operators
### Source: PowerShell
```powershell
function Method
{
	[bool]$eq = 1 -eq 2
	[bool]$notEq = 1 -ne 2
	[bool]$or = 1 -eq 2 -or 2 -eq 1
	[bool]$and = 1 -eq 2 -and 2 -eq 1
	[bool]$gt = 1 -gt 2
	[bool]$lt = 1 -lt 2
	[bool]$ge = 1 -ge 2
	[bool]$le = 1 -le 2
	[int]$plus = 1 + 1
	[int]$minus = 1 - 1
	[int]$bor = 1 -bor 1
}
```
### Target: CSharp
```csharp
void Method()
{
	bool eq = 1 == 2;
	bool notEq = 1 != 2;
	bool or = 1 == 2 || 2 == 1;
	bool and = 1 == 2 && 2 == 1;
	bool gt = 1 > 2;
	bool lt = 1 < 2;
	bool ge = 1 >= 2;
	bool le = 1 <= 2;
	int plus = 1 + 1;
	int minus = 1 - 1;
	int bor = 1 | 1;
}
```

# Convert PowerShell to CSharp
## Return statement
### Source: PowerShell
```powershell
function Method
{
	return 1
}
```
### Target: CSharp
```csharp
void Method()
{
	return 1;
}
```

# Convert PowerShell to CSharp
## Declare a method
### Source: PowerShell
```powershell
function Method
{
}
```
### Target: CSharp
```csharp
void Method()
{
}
```

# Convert PowerShell to CSharp
## Declare a method with arguments
### Source: PowerShell
```powershell
function Method
{
	param([string]$argument, [int]$integer)
}
```
### Target: CSharp
```csharp
void Method(String argument, Int32 integer)
{
}
```

# Convert PowerShell to CSharp
## Switch statement
### Source: PowerShell
```powershell
function Method
{
	[int]$i = 0
	[int]$x = 1
	switch ($i)
	{
		2 { $x = 2 }
		3 { $x = 3 }
		default { }
	}
}
```
### Target: CSharp
```csharp
void Method()
{
	int i = 0;
	int x = 1;
	switch (i)
	{
		case 2:
			x = 2;
			break;
		case 3:
			x = 3;
			break;
		default:
			break;
	}
}
```

# Convert PowerShell to CSharp
## Foreach loop
### Source: PowerShell
```powershell
function Method
{
	param([string[]]$strings)
	foreach($item in $strings)
	{
		[string]$str = $item
		continue
	}
}
```
### Target: CSharp
```csharp
void Method(String[] strings)
{
	foreach (var item in strings)
	{
		string str = item;
		continue;
	}
}
```

# Convert PowerShell to CSharp
## Indexer property
### Source: PowerShell
```powershell
function Method
{
	param([string]$str)
	[string]$item = $str[3]
}
```
### Target: CSharp
```csharp
void Method(String str)
{
	string item = str[3];
}
```

# Convert PowerShell to CSharp
## For loop
### Source: PowerShell
```powershell
function Method
{
	for([int]$i = 0; $i -lt 100; $i++)
	{
		[int]$t = $i
	}
}
```
### Target: CSharp
```csharp
void Method()
{
	for(int i = 0; i < 100; i++)
	{
		int t = i;
	}
}
```

# Convert PowerShell to CSharp
## Create an object
### Source: PowerShell
```powershell
function Method
{
	New-Object -TypeName System.Object
}
```
### Target: CSharp
```csharp
void Method()
{
	new System.Object();
}
```

# Convert PowerShell to CSharp
## Try, catch, finally
### Source: PowerShell
```powershell
function Method
{
	try
	{
		$item = New-Object -TypeName object
	}
	catch [Exception]
	{
		$item = New-Object -TypeName object
	}
	catch
	{
		$item = New-Object -TypeName object
	}
	finally
	{
		$item = New-Object -TypeName object
	}
}
```
### Target: CSharp
```csharp
void Method()
{
	try
	{
		item = new object();
	}
	catch (Exception)
	{
		item = new object();
	}
	catch
	{
		item = new object();
	}
	finally
	{
		item = new object();
	}
}
```

# Convert PowerShell to CSharp
## Cast operator
### Source: PowerShell
```powershell
function Method
{
	[int]$myInt = 1
	[long]$myLong = [long]$myInt
}
```
### Target: CSharp
```csharp
void Method()
{
	int myInt = 1;
	long myLong = (long)myInt;
}
```

# Convert PowerShell to CSharp
## Array creation initializers
### Source: PowerShell
```powershell
function Method
{
	[string[]]$arr = @("my","strings")
}
```
### Target: CSharp
```csharp
void Method()
{
	string[] arr = new [] { "my", "strings" };
}
```

# Convert PowerShell to CSharp
## Assign a constant to a variable
### Source: PowerShell
```powershell
function Method
{
	[int]$variable = 1
	[string]$variable2 = "myString"
}
```
### Target: CSharp
```csharp
void Method()
{
	int variable = 1;
	string variable2 = "myString";
}
```

# Convert PowerShell to CSharp
## Access the property of a variable
### Source: PowerShell
```powershell
function Method
{
	[TimeZoneInfo]$timeZoneInfo = New-Object -TypeName TimeZoneInfo
	[string]$variable = $timeZoneInfo.DisplayName
}
```
### Target: CSharp
```csharp
void Method()
{
	TimeZoneInfo timeZoneInfo = new TimeZoneInfo();
	string variable = timeZoneInfo.DisplayName;
}
```

# Convert PowerShell to CSharp
## Throw statement
### Source: PowerShell
```powershell
function Method
{
	throw (New-Object -TypeName Exception -ArgumentList "Hey")
}
```
### Target: CSharp
```csharp
void Method()
{
	throw (new Exception("Hey"));
}
```

# Convert PowerShell to CSharp
## Create an object with arugments
### Source: PowerShell
```powershell
function Method
{
	New-Object -TypeName System.Object -ArgumentList $myVariable
}
```
### Target: CSharp
```csharp
void Method()
{
	new System.Object(myVariable);
}
```

# Convert PowerShell to CSharp
## If, Else If, Else
### Source: PowerShell
```powershell
function Method
{
	if (1 -eq 2)
	{
	}
	elseif ("xyz" -eq (New-Object -TypeName Object))
	{
	}
	else
	{
	}
}
```
### Target: CSharp
```csharp
void Method()
{
	if (1 == 2)
	{
	}
	else if ("xyz" == (new Object()))
	{
	}
	else
	{
	}
}
```
