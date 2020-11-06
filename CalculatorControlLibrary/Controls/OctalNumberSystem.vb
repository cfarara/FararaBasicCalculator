Imports CalculatorMathLogic
''' <summary>
''' The mathematical numbering system Octal.
''' </summary>
Public Class OctalNumberSystem


  Public Shadows Const MAX_DIGITS As Integer = 30

  ''' <summary>
  ''' Evaluates the expression in the current user input history.
  ''' </summary>
  Protected Overrides Sub btnEquals_Click(sender As Object, e As EventArgs)
    If (Me.txtUserEvaluationHistory.Text.Equals(String.Empty)) Then
      Me.lvwHistory.Items.Add(Me.txtAnswer.Text & EQUALS_OPERATION & Me.txtAnswer.Text)
      Me.ClearEvaluationHistory()
    Else
      Me.txtUserEvaluationHistory.Text &= Me.txtAnswer.Text
      Me.Evaluate(MathLogic.SolveEquationOctal(Me.txtUserEvaluationHistory.Text))
    End If
  End Sub
End Class
