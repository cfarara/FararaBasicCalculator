''' <summary>
''' The Numbering System which defines a basic calculator number screen.
''' </summary>
''' Mark Must Inherit
Public MustInherit Class NumberingSystem


  Public Const MAX_DIGITS As Integer = 32
  Public Const PERIOD As String = "."
  Public Const ADD_OPERATION As String = " + "
  Public Const MINUS_OPERATION As String = " - "
  Public Const MULTIPLY_OPERATION As String = " * "
  Public Const BASE10_DIVISION_OPERATION As String = " / "
  Public Const DIVISION_OPERATION As String = " \ "
  Public Const EQUALS_OPERATION As String = " = "

  ''' <summary>
  ''' Tells whether data in answer textbox is old or new.
  ''' </summary>
  ''' <returns>True if the user has updated the answer text box by entering input, false otherwise</returns>
  Public Property UpdateInput As Boolean = False

  Public Sub New()

    ' This call is required by the designer.
    InitializeComponent()

    ' Add any initialization after the InitializeComponent() call.
    Me.lvwHistory.Columns.Add("Evaluations", 1000, HorizontalAlignment.Left)
  End Sub

  ''' <summary>
  ''' Determines whether the mathematical expression ends in a mathematical operation
  ''' </summary>
  ''' <param name="sPhrase">the mathematical expression to check</param>
  ''' <returns>True if the mathematical expression ends in an operation, false otherwise</returns>
  Public Function ExpressionEndsInOperation(sPhrase As String) As Boolean
    If sPhrase.Length >= 4 Then
      If sPhrase.EndsWith(ADD_OPERATION) OrElse sPhrase.EndsWith(MINUS_OPERATION) _
        OrElse sPhrase.EndsWith(MULTIPLY_OPERATION) OrElse sPhrase.EndsWith(DIVISION_OPERATION) _
        OrElse sPhrase.EndsWith(BASE10_DIVISION_OPERATION) Then
        Return True
      End If
    End If
    Return False
  End Function

  ''' <summary>
  ''' Clears the current evaluation (evaluation history AND current answer)
  ''' </summary>
  Public Sub ClearCurrentEvaluation()
    Me.txtUserEvaluationHistory.Text = String.Empty
    Me.txtAnswer.Text = "0"
  End Sub

  ''' <summary>
  ''' Clears the user input history, current answer, and all evaluation history
  ''' </summary>
  Public Sub Clear()
    Me.ClearEvaluationHistory()
    Me.ClearAnswer()
  End Sub

  ''' <summary>
  ''' Clears only the current evaluation history
  ''' </summary>
  Public Sub ClearEvaluationHistory()
    Me.txtUserEvaluationHistory.Text = String.Empty
  End Sub

  ''' <summary>
  ''' Clears the answer text ( resets to 0 ) and readys for new input.
  ''' </summary>
  Public Sub ClearAnswer()
    Me.txtAnswer.Text = "0"
    Me.UpdateInput = True
  End Sub

  ''' <summary>
  ''' Clears all previous evaluation history in listview.
  ''' </summary>
  Public Sub ClearHistory()
    Me.lvwHistory.Items.Clear()
  End Sub

  ''' <summary>
  ''' Adds the specified input to the answer text box.
  ''' </summary>
  ''' <param name="sInput">Input from the key/button pressed/clicked by the user</param>
  Public Sub AddInputToAnswerBox(sInput As String)
    If Not (Me.UpdateInput) Then
      Me.txtAnswer.Text = sInput
    ElseIf (Me.txtAnswer.Text.Length >= MAX_DIGITS) OrElse
            (Me.txtAnswer.Text.Contains(PERIOD) AndAlso sInput.Equals(PERIOD)) Then
      Return
    ElseIf (Me.txtAnswer.Text.Equals("0") OrElse Me.txtAnswer.Text.Equals(Nothing)) Then
      Me.txtAnswer.Text = sInput
    Else
      Me.txtAnswer.Text &= sInput
    End If
    Me.UpdateInput = True
  End Sub

  ''' <summary>
  ''' Removes the last number in the answer
  ''' </summary>
  Public Sub RemoveLastNumber()
    If Not (Me.UpdateInput) Then
      Return
    ElseIf Not (Me.txtAnswer.Text.Length <= 1) Then
      Me.txtAnswer.Text = Me.txtAnswer.Text.Remove(Me.txtAnswer.Text.Length - 1)
    ElseIf Not (Me.txtAnswer.Text.Equals("0")) Then
      Me.txtAnswer.Text = "0"
    End If
    Me.UpdateInput = True
  End Sub

  Private Declare Function HideCaret Lib "user32.dll" (ByVal hWnd As IntPtr) As Boolean

  ''' <summary>
  ''' Adds an operation to the evaluation then waits for input.
  ''' </summary>
  ''' <param name="sOperation"></param>
  Public Sub AddAnOperation(sOperation As String)
    ' Check if input history is empty, then checks if the current evaluation ends with an operation 
    ' - if yes, check if incoming operation is the same 
    '   - if yes, check if user has updated the input
    '       - if yes, add the answer box number and the new operation
    '       - else, replace the old operation with the new operation
    '   - else(false)
    '       - check if user has updated the input
    '         - if yes, add the answer box number and the operation
    ' - else(expression does not end in an operation), add the operation
    '   - add the new operation
    If (Me.txtUserEvaluationHistory.Text Is Nothing OrElse
      Me.txtUserEvaluationHistory.Text.Equals(String.Empty)) Then
      Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text & sOperation
    ElseIf Me.ExpressionEndsInOperation(Me.txtUserEvaluationHistory.Text) Then
      If Not (Me.txtUserEvaluationHistory.Text.Chars(Me.txtUserEvaluationHistory.Text.Length - 2).Equals(sOperation.Chars(1))) Then
        If Not Me.UpdateInput Then
          Me.txtUserEvaluationHistory.Text = Me.txtUserEvaluationHistory.Text.Substring(0, Me.txtUserEvaluationHistory.Text.Length - 3) & sOperation
        Else
          Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text & sOperation
        End If
      Else
        If Not Me.UpdateInput Then
          Return
        Else
          Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text & sOperation
        End If
      End If
    Else
      Me.txtUserEvaluationHistory.Text &= sOperation
    End If
  End Sub

  Private Sub btnOperation_Click(sender As Object, e As EventArgs) Handles btnAdd.Click, btnSubtract.Click, btnMultiply.Click, btnDivide.Click
    Dim btnCurrent As Button = CType(sender, Button)

    Select Case True
      Case btnCurrent.Equals(btnAdd)
        Me.AddAnOperation(ADD_OPERATION)
      Case btnCurrent.Equals(btnSubtract)
        Me.AddAnOperation(MINUS_OPERATION)
      Case btnCurrent.Equals(btnMultiply)
        Me.AddAnOperation(MULTIPLY_OPERATION)
      Case btnCurrent.Equals(btnDivide)
        If Me.GetType Is GetType(Base10NumberSystem) Then
          Me.AddAnOperation(BASE10_DIVISION_OPERATION)
        Else
          Me.AddAnOperation(DIVISION_OPERATION)
        End If
    End Select
    Me.FocusOnEqualsButton()
    Me.UpdateInput = False
  End Sub

  Private Sub txtUserInputHistory_GotFocus(sender As Object, e As EventArgs) Handles txtUserEvaluationHistory.GotFocus
    HideCaret(txtUserEvaluationHistory.Handle)
  End Sub

  Private Sub txtAnswer_GotFocus(sender As Object, e As EventArgs) Handles txtAnswer.GotFocus
    HideCaret(txtAnswer.Handle)
  End Sub

  Private Sub btnClear_Click(sender As Object, e As EventArgs) Handles btnCLear.Click
    Me.ClearCurrentEvaluation()
    Me.FocusOnEqualsButton()
  End Sub

  Private Sub btnClearAnswer_Click(sender As Object, e As EventArgs) Handles btnClearAnswer.Click
    Me.ClearAnswer()
    Me.FocusOnEqualsButton()
  End Sub

  Private Sub btnBackspace_Click(sender As Object, e As EventArgs) Handles btnBackspace.Click
    Me.RemoveLastNumber()
    Me.FocusOnEqualsButton()
  End Sub

  Private Sub btnInput_Click(sender As Object, e As EventArgs) Handles btnPeriod.Click, btnZero.Click, btnOne.Click, btnTwo.Click,
  btnThree.Click, btnFour.Click, btnFive.Click, btnSix.Click, btnSeven.Click, btnEight.Click, btnNine.Click,
  btnA.Click, btnB.Click, btnC.Click, btnD.Click, btnE.Click, btnF.Click, btnG.Click, btnH.Click, btnJ.Click,
  btnK.Click, btnL.Click, btnM.Click, btnN.Click, btnP.Click, btnQ.Click, btnR.Click, btnS.Click, btnT.Click,
  btnV.Click, btnW.Click, btnX.Click, btnY.Click, btnZ.Click

    Dim btnCurrent As Button = CType(sender, Button)

    Me.AddInputToAnswerBox(btnCurrent.Text)
    Me.FocusOnEqualsButton()
  End Sub

  Private Sub FocusOnEqualsButton()
    Me.btnEquals.Select()
  End Sub

  ''' <summary>
  ''' Process input when the user presses a key
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  Public Sub NumberingSystem_KeyDown(sender As Object, e As KeyEventArgs)
    Select Case e.KeyCode
      Case Keys.Back
        Me.RemoveLastNumber()
      Case Keys.Escape
        Me.ClearEvaluationHistory()
      Case Keys.OemPeriod
        If Me.btnPeriod.Enabled Then
          Me.AddInputToAnswerBox(PERIOD)
        End If
      Case Keys.Decimal
        If Me.btnPeriod.Enabled Then
          Me.AddInputToAnswerBox(PERIOD)
        End If
      Case Keys.Add
        Me.AddAnOperation(ADD_OPERATION)
        Me.UpdateInput = False
      Case Keys.Oemplus
        Me.AddAnOperation(ADD_OPERATION)
        Me.UpdateInput = False
      Case Keys.Subtract
        Me.AddAnOperation(MINUS_OPERATION)
        Me.UpdateInput = False
      Case Keys.OemMinus
        Me.AddAnOperation(MINUS_OPERATION)
        Me.UpdateInput = False
      Case Keys.Multiply
        Me.AddAnOperation(MULTIPLY_OPERATION)
        Me.UpdateInput = False
      Case Keys.Divide
        If Me.GetType Is GetType(Base10NumberSystem) Then
          Me.AddAnOperation(BASE10_DIVISION_OPERATION)
          Me.UpdateInput = False
        Else
          Me.AddAnOperation(DIVISION_OPERATION)
          Me.UpdateInput = False
        End If
      Case Keys.Enter
        btnEquals_Click(sender, e)
      Case Keys.Return
        btnEquals_Click(sender, e)
      Case Keys.A
        If Me.btnA.Enabled Then
          Me.AddInputToAnswerBox("A")
        End If
      Case Keys.B
        If Me.btnB.Enabled Then
          Me.AddInputToAnswerBox("B")
        End If
      Case Keys.C
        If Me.btnC.Enabled Then
          Me.AddInputToAnswerBox("C")
        End If
      Case Keys.D
        If Me.btnD.Enabled Then
          Me.AddInputToAnswerBox("D")
        End If
      Case Keys.E
        If Me.btnE.Enabled Then
          Me.AddInputToAnswerBox("E")
        End If
      Case Keys.F
        If Me.btnF.Enabled Then
          Me.AddInputToAnswerBox("F")
        End If
      Case Keys.G
        If Me.btnG.Enabled Then
          Me.AddInputToAnswerBox("G")
        End If
      Case Keys.H
        If Me.btnH.Enabled Then
          Me.AddInputToAnswerBox("H")
        End If
      Case Keys.J
        If Me.btnJ.Enabled Then
          Me.AddInputToAnswerBox("J")
        End If
      Case Keys.K
        If Me.btnK.Enabled Then
          Me.AddInputToAnswerBox("K")
        End If
      Case Keys.L
        If Me.btnL.Enabled Then
          Me.AddInputToAnswerBox("L")
        End If
      Case Keys.M
        If Me.btnM.Enabled Then
          Me.AddInputToAnswerBox("M")
        End If
      Case Keys.N
        If Me.btnN.Enabled Then
          Me.AddInputToAnswerBox("N")
        End If
      Case Keys.P
        If Me.btnP.Enabled Then
          Me.AddInputToAnswerBox("P")
        End If
      Case Keys.Q
        If Me.btnQ.Enabled Then
          Me.AddInputToAnswerBox("Q")
        End If
      Case Keys.R
        If Me.btnR.Enabled Then
          Me.AddInputToAnswerBox("R")
        End If
      Case Keys.S
        If Me.btnS.Enabled Then
          Me.AddInputToAnswerBox("S")
        End If
      Case Keys.T
        If Me.btnT.Enabled Then
          Me.AddInputToAnswerBox("T")
        End If
      Case Keys.V
        If Me.btnV.Enabled Then
          Me.AddInputToAnswerBox("V")
        End If
      Case Keys.W
        If Me.btnW.Enabled Then
          Me.AddInputToAnswerBox("W")
        End If
      Case Keys.X
        If Me.btnX.Enabled Then
          Me.AddInputToAnswerBox("X")
        End If
      Case Keys.Y
        If Me.btnY.Enabled Then
          Me.AddInputToAnswerBox("Y")
        End If
      Case Keys.Z
        If Me.btnZ.Enabled Then
          Me.AddInputToAnswerBox("Z")
        End If
      Case Keys.D0
        If Me.btnZero.Enabled Then
          Me.AddInputToAnswerBox("0")
        End If
      Case Keys.NumPad0
        If Me.btnZero.Enabled Then
          Me.AddInputToAnswerBox("0")
        End If
      Case Keys.D1
        If Me.btnOne.Enabled Then
          Me.AddInputToAnswerBox("1")
        End If
      Case Keys.NumPad1
        If Me.btnOne.Enabled Then
          Me.AddInputToAnswerBox("1")
        End If
      Case Keys.D2
        If Me.btnTwo.Enabled Then
          Me.AddInputToAnswerBox("2")
        End If
      Case Keys.NumPad2
        If Me.btnTwo.Enabled Then
          Me.AddInputToAnswerBox("2")
        End If
      Case Keys.D3
        If Me.btnThree.Enabled Then
          Me.AddInputToAnswerBox("3")
        End If
      Case Keys.NumPad3
        If Me.btnThree.Enabled Then
          Me.AddInputToAnswerBox("3")
        End If
      Case Keys.D4
        If Me.btnFour.Enabled Then
          Me.AddInputToAnswerBox("4")
        End If
      Case Keys.NumPad4
        If Me.btnFour.Enabled Then
          Me.AddInputToAnswerBox("4")
        End If
      Case Keys.D5
        If Me.btnFive.Enabled Then
          Me.AddInputToAnswerBox("5")
        End If
      Case Keys.NumPad5
        If Me.btnFive.Enabled Then
          Me.AddInputToAnswerBox("5")
        End If
      Case Keys.D6
        If Me.btnSix.Enabled Then
          Me.AddInputToAnswerBox("6")
        End If
      Case Keys.NumPad6
        If Me.btnSix.Enabled Then
          Me.AddInputToAnswerBox("6")
        End If
      Case Keys.D7
        If Me.btnSeven.Enabled Then
          Me.AddInputToAnswerBox("7")
        End If
      Case Keys.NumPad7
        If Me.btnSeven.Enabled Then
          Me.AddInputToAnswerBox("7")
        End If
      Case Keys.D8
        If Me.btnEight.Enabled Then
          Me.AddInputToAnswerBox("8")
        End If
      Case Keys.NumPad8
        If Me.btnEight.Enabled Then
          Me.AddInputToAnswerBox("8")
        End If
      Case Keys.D9
        If Me.btnNine.Enabled Then
          Me.AddInputToAnswerBox("9")
        End If
      Case Keys.NumPad9
        If Me.btnNine.Enabled Then
          Me.AddInputToAnswerBox("9")
        End If
      Case Else
        e.SuppressKeyPress = False
    End Select
  End Sub

  Private Sub btnClearHistory_Click(sender As Object, e As EventArgs) Handles btnClearHistory.Click
    Me.ClearHistory()
    Me.FocusOnEqualsButton()
  End Sub

  Private Sub lvwHistory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwHistory.SelectedIndexChanged
    Try
      Dim sSelectedString As String = Me.lvwHistory.Items(Me.lvwHistory.FocusedItem.Index).Text
      Dim ssSelectedString As String() = sSelectedString.Split({" = "}, StringSplitOptions.None)
      Me.txtUserEvaluationHistory.Text = ssSelectedString(0)
      Me.txtAnswer.Text = ssSelectedString(1)
      Me.UpdateInput = False
    Catch ex As Exception
      Me.txtAnswer.Text = "Operation failed."
      Me.UpdateInput = False
    End Try
  End Sub

  Protected MustOverride Sub btnEquals_Click(sender As Object, e As EventArgs) Handles btnEquals.Click

  ''' <summary>
  ''' Evaluates the mathematical expression in the current evaluation.
  ''' </summary>
  ''' Mark Must Override
  Protected Sub Evaluate(sEquationToSove As String)
    Select Case True
      Case (Me.txtUserEvaluationHistory.Text.Equals(String.Empty))
        Me.lvwHistory.Items.Add(Me.txtAnswer.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
        Me.ClearEvaluationHistory()
      Case Me.UpdateInput
        Try
          Me.txtAnswer.Text = sEquationToSove
          Me.lvwHistory.Items.Add(Me.txtUserEvaluationHistory.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
        Catch aE As Exception
          MessageBox.Show("The Equation specified cannot be completed because it exceeds the application workflow.", "Error: 10001", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          Me.ClearAnswer()
        Finally
          Me.ClearEvaluationHistory()
          Me.UpdateInput = False
        End Try
      Case Me.ExpressionEndsInOperation(Me.txtUserEvaluationHistory.Text)
        Me.txtUserEvaluationHistory.Text = Me.txtUserEvaluationHistory.Text.Substring(0, Me.txtUserEvaluationHistory.Text.Length - 3)
        Try
          Me.txtAnswer.Text = sEquationToSove
          Me.lvwHistory.Items.Add(Me.txtUserEvaluationHistory.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
        Catch aE As Exception
          MessageBox.Show("The Equation specified cannot be completed because it exceeds the application workflow.", "Error: 10001", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
          Me.ClearAnswer()
        Finally
          Me.ClearEvaluationHistory()
          Me.UpdateInput = False
        End Try
    End Select
  End Sub
End Class
