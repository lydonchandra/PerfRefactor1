using System.Text;
using DnaLib;

var path = "Data/gene-xl.fna";

using FileStream fileStream = new(path, FileMode.Open, FileAccess.Read);
using var streamReader = new StreamReader(fileStream);
var data = await streamReader.ReadToEndAsync();
var dataBytes = Encoding.UTF8.GetBytes(data);

// var valid = DnaUtil.ValidateDnaVec256(dataBytes.AsSpan());
// Console.WriteLine(valid);

// var valid = DnaUtil.ValidateDnaVec128(dataBytes.AsSpan());
// Console.WriteLine(valid);

var valid = DnaUtil.ValidateDnaVec384(dataBytes.AsSpan());
Console.WriteLine(valid);