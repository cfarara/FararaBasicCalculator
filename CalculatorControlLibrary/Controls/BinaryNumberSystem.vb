Imports CalculatorMathLogic
''' <summary>
''' The mathematical numbering system Binary.
''' </summary>
Public Class BinaryNumberSystem

  Public Shadows Const MAX_DIGITS As Integer = 32

  Protected Overrides Sub btnEquals_Click(sender As Object, e As EventArgs)
    If (Me.txtUserEvaluationHistory.Text.Equals(String.Empty)) Then
      Me.lvwHistory.Items.Add(Me.txtAnswer.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
      Me.ClearEvaluationHistory()
    Else
      Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text
      Me.Evaluate(MathLogic.SolveEquationBinary(Me.txtUserEvaluationHistory.Text))
    End If
  End Sub

End Class
