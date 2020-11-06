Imports CalculatorMathLogic
''' <summary>
''' The mathematical numbering system Base10.
''' </summary>
Public Class Base10NumberSystem

  Public Shadows Const MAX_DIGITS As Integer = 20

  Protected Overrides Sub btnEquals_Click(sender As Object, e As EventArgs)
    If (Me.txtUserEvaluationHistory.Text.Equals(String.Empty)) Then
      Me.lvwHistory.Items.Add(Me.txtAnswer.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
      Me.ClearEvaluationHistory()
    Else
      Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text
      Me.Evaluate(MathLogic.SolveEquationBase10(Me.txtUserEvaluationHistory.Text))
    End If
  End Sub

End Class
