namespace ZigZag;

public class ZigZag {
    const int MaxNumRows = 1000;
    const int MinNumRows = 1;
    
    public Tuple<bool, string> Validate(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return new (false, "empty input");
        }
        bool isValid = true;
        for (var i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (char.IsAsciiLetter(c) || c == ',' || c == '.')
            {
                continue;
            }
            else
            {
                isValid = false;
                break;
            }
        }

        // todo: better error message, tell offending char
        
        if (!isValid) return new(false, "invalid input");
        
        return new (true, string.Empty);
    }

    //Input: s = "PAYPALISHIRING", numRows = 4
    //Output: "PINALSIGYAHRPI"
    //Explanation:
    //P  I  N
    //A LS IG
    //YA HR
    //P  I
    public string Convert(string input, int numRows) {
        var (isValid, errorMsg) = this.Validate(input);
        if (!isValid) {
            throw new ArgumentException(errorMsg);
        }
        if(numRows < MinNumRows || numRows > MaxNumRows) {
            //todo: better error message
            throw new ArgumentException("Invalid numRows");
        }
                
        List<string> columns = new();                
        for(int i=0; i < input.Length;) {            
            if (i == 0 
                || i % ((numRows - 1)*2) == 0) 
            {
                string column = string.Empty;

                for (int j = 0; j < numRows; j++)
                {
                    if ((i+j) >= input.Length) {
                        column += " ";
                        continue;
                    }
                    char elem = input[i+j];
                    column += elem;
                }
                i += numRows;
                columns.Add(column);                
            }      
            else {
                int targetIdx = (columns.Count * (numRows - 1)) + numRows - (i % (numRows-1));
                string column = string.Empty;
                int startIdx = columns.Count * (numRows-1) + 1;
                for(int j=startIdx; j < startIdx+numRows; j++) {
                    int elemIdx = j;
                    if(elemIdx == targetIdx) {
                        column += input[i];
                    }
                    else {
                        column += " ";
                    }
                }
                columns.Add(column);
                i += 1;
            }
        }
        string output = string.Empty;
        for(int i=0; i<numRows; i++) {
            for(int j=0; j<columns.Count; j++) {
                char elem = columns[j][i];
                if(elem == ' ') {
                    continue;
                }
                output += columns[j][i];
            }
        }
        return output;
        
    }
}
