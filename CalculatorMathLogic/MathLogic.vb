''' <summary>
''' Math Logic processes mathetical expressions.
''' </summary>
Public Module MathLogic

  Public Const ADD_OPERATION As String = "+"
  Public Const MINUS_OPERATION As String = "-"
  Public Const MULTIPLY_OPERATION As String = "*"
  Public Const BASE10_DIVISION_OPERATION As String = "/"
  Public Const DIVISION_OPERATION As String = "\"

  ''' <summary>
  ''' Solves the specified equation using the Base10 Number System
  ''' </summary>
  ''' <param name="sEquation">the equation which to solve</param>
  ''' <returns>the answer to the equation</returns>
  Public Function SolveEquationBase10(sEquation As String) As String
    Dim lAnswer As Decimal = 0
    Dim sOperation As String = String.Empty
    Dim asExpression As String() = sEquation.Split({" "}, StringSplitOptions.None)

    For i As Integer = 0 To asExpression.Length - 1 Step 1
      If i = 0 Then
        lAnswer = Decimal.Parse(asExpression(i))
        Continue For
      End If
      If Not IsNumeric(asExpression(i)) Then
        sOperation = asExpression(i)
      ElseIf (i Mod 2) = 0 Then
        lAnswer = DoAppropriateOperation(sOperation, lAnswer, CDec(asExpression(i)))
      End If
    Next

    Return CStr(lAnswer)
  End Function

  ''' <summary>
  ''' Determines which mathematical operation to operate based on the specified operation
  ''' </summary>
  ''' <param name="sOperation">The operation to do</param>
  ''' <param name="iNum1">the first number</param>
  ''' <param name="iNum2">the second number</param>
  ''' <returns>the answer derived from the specified operation and num1 and num2</returns>
  Public Function DoAppropriateOperation(sOperation As String, iNum1 As Decimal, iNum2 As Decimal) As Decimal
    Select Case sOperation
      Case ADD_OPERATION
        Return iNum1 + iNum2
      Case MINUS_OPERATION
        Return iNum1 - iNum2
      Case MULTIPLY_OPERATION
        Return iNum1 * iNum2
      Case DIVISION_OPERATION
        Return CLng(iNum1) \ CLng(iNum2)
      Case BASE10_DIVISION_OPERATION
        Return iNum1 / iNum2
    End Select
    Return 0
  End Function

  ''' <summary>
  ''' Solves the specified equation using the Binary Number System
  ''' </summary>
  ''' <param name="sEquation"></param>
  ''' <returns>the answer to the equation</returns>
  Public Function SolveEquationBinary(sEquation As String) As String
    Dim lAnswer As Decimal = 0
    Dim sOperation As String = String.Empty
    Dim asExpression As String() = sEquation.Split({" "}, StringSplitOptions.None)

    For i As Integer = 0 To asExpression.Length - 1 Step 1
      If i = 0 Then
        lAnswer = ConvertBinaryToInt(asExpression(i))
        Continue For
      End If
      If Not IsNumeric(asExpression(i)) Then
        sOperation = asExpression(i)
      ElseIf (i Mod 2) = 0 Then
        lAnswer = DoAppropriateOperation(sOperation, lAnswer, ConvertBinaryToInt(asExpression(i)))
      End If
    Next

    Return ConvertIntegerToBinary(lAnswer)
  End Function

  ''' <summary>
  ''' Converts a binary string to an Integer
  ''' </summary>
  ''' <param name="sBinaryNumber">the string binary to convert</param>
  ''' <returns>The integer representation of the binary string</returns>
  Public Function ConvertBinaryToInt(sBinaryNumber As String) As Integer

    Dim iLength As Integer
    'Get the length of the binary string
    iLength = Len(sBinaryNumber)
    Dim TempValue As Double

    'Convert each binary digit to its corresponding integer value
    'and add the value to the previous sum
    'The string is parsed from the right (LSB - Least Significant Bit)
    'to the left (MSB - Most Significant Bit)
    For x As Integer = 1 To iLength
      TempValue = TempValue + Val(Mid(sBinaryNumber, iLength - x + 1, 1)) * 2 ^ (x - 1)
    Next

    ConvertBinaryToInt = CInt(TempValue)

  End Function

  ''' <summary>
  ''' Converts a Integer into a binary string
  ''' </summary>
  ''' <param name="lIntegerNumber">the Integer to convert to binary string</param>
  ''' <returns>the binary string representation of the Integer</returns>
  Public Function ConvertIntegerToBinary(lIntegerNumber As Decimal) As String

    Dim lIntNum As Decimal
    lIntNum = lIntegerNumber
    Dim TempValue As Decimal
    Dim sBinValue As String = String.Empty

    Do
      'Use the Mod operator to get the current binary digit from the
      'Integer number
      TempValue = lIntNum Mod 2
      sBinValue = CStr(TempValue) + sBinValue

      'Divide the current number by 2 and get the integer result
      lIntNum = CLng(lIntNum) \ 2
    Loop Until lIntNum = 0

    ConvertIntegerToBinary = sBinValue

  End Function

  ''' <summary>
  ''' Solves the specified equation using the Octal Number System
  ''' </summary>
  ''' <param name="sEquation">the equation to solve</param>
  ''' <returns></returns>
  Public Function SolveEquationOctal(sEquation As String) As String
    Dim lAnswer As Decimal = 0
    Dim lNextNum As Decimal = 0
    Dim sOperation As String = String.Empty
    Dim asExpression As String() = sEquation.Split({" "}, StringSplitOptions.None)

    For i As Integer = 0 To asExpression.Length - 1 Step 1
      If i = 0 Then
        lAnswer = ConvertOctalToInt(asExpression(i))
        Continue For
      End If
      If Not IsNumeric(asExpression(i)) Then
        sOperation = asExpression(i)
      ElseIf (i Mod 2) = 0 Then
        lNextNum = ConvertOctalToInt(asExpression(i))
        lAnswer = DoAppropriateOperation(sOperation, lAnswer, lNextNum)
      End If
    Next

    Return CStr(ConvertIntToOctal(lAnswer))
  End Function

  ''' <summary>
  ''' Converts an octal string into an Integer
  ''' </summary>
  ''' <param name="sNumber">the octal string to convert</param>
  ''' <returns>the integer representation of the octal string</returns>
  Public Function ConvertOctalToInt(sNumber As String) As Decimal
    Return CLng("&O" & sNumber)
  End Function

  ''' <summary>
  ''' Converts an Integer into an octal string 
  ''' </summary>
  ''' <param name="lNumberToConvert">the integer to convert</param>
  ''' <returns></returns>
  Public Function ConvertIntToOctal(lNumberToConvert As Decimal) As Decimal
    If lNumberToConvert < 1 Then
      Return 0
    End If

    Dim sOctStr As String = String.Empty

    While lNumberToConvert > 0
      sOctStr = sOctStr.Insert(0, (lNumberToConvert Mod 8).ToString())
      lNumberToConvert = Int(lNumberToConvert / 8)
    End While

    Return CDec(sOctStr)
  End Function

  ''' <summary>
  ''' Solves the specified equation using the Hexadecimal Number System
  ''' </summary>
  ''' <param name="sEquation">the specified equation</param>
  ''' <returns>the answer to the equation</returns>
  Public Function SolveEquationHexadecimal(sEquation As String) As String
    Dim lAnswer As Decimal = 0
    Dim lNextNum As Decimal = 0
    Dim sOperation As String = String.Empty
    Dim asExpression As String() = sEquation.Split({" "}, StringSplitOptions.None)

    For i As Integer = 0 To asExpression.Length - 1 Step 1
      If i = 0 Then
        lAnswer = Convert.ToInt64(asExpression(i), 16)
        Continue For
      End If
      If asExpression(i).Equals(ADD_OPERATION) OrElse asExpression(i).Equals(MINUS_OPERATION) _
        OrElse asExpression(i).Equals(MULTIPLY_OPERATION) OrElse asExpression(i).Equals(DIVISION_OPERATION) Then
        sOperation = asExpression(i)
      ElseIf (i Mod 2) = 0 Then
        lNextNum = Convert.ToInt64(asExpression(i), 16)
        lAnswer = DoAppropriateOperation(sOperation, lAnswer, lNextNum)
      End If
    Next

    Return Hex(lAnswer)
  End Function

End Module
