Imports CalculatorMathLogic
''' <summary>
''' The mathematical numbering system Hexadecimal.
''' </summary>
Public Class HexadecimalNumberSystem

  Public Shadows Const MAX_DIGITS As Integer = 22

  Protected Overrides Sub btnEquals_Click(sender As Object, e As EventArgs)
    If (Me.txtUserEvaluationHistory.Text.Equals(String.Empty)) Then
      Me.lvwHistory.Items.Add(Me.txtAnswer.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
      Me.ClearEvaluationHistory()
    Else
      Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text
      Me.Evaluate(MathLogic.SolveEquationHexadecimal(Me.txtUserEvaluationHistory.Text))
    End If
  End Sub
End Class
