Imports System.Reflection

Namespace MyIdealProp.Workflow.Core.Model
    <Serializable>
    Public Class LineInstance
        Public Property LineInstId As Integer
        Friend Property LineDefinition As LineDefinition
        Public Property ProcessInstance As ProcessInstance
        Public Property LineRuleResult As Boolean

        Friend Shared Function Create(procInst As ProcessInstance, line As LineDefinition) As LineInstance
            Return New LineInstance With
                   {
                       .LineDefinition = line,
                       .ProcessInstance = procInst
                       }
        End Function

        Friend Sub Execute()
            Dim action = LineDefinition.CodeRule
            Dim type__1 = action.Type
            If type__1 Is Nothing Then
                Throw New InvalidOperationException()
            End If


            Dim methodInfo = type__1.GetMethod(action.MethodName)

            If methodInfo.IsStatic Then
                Dim a = methodInfo.Invoke(Nothing, {Me})
            Else
                Dim obj = Activator.CreateInstance(type__1, BindingFlags.NonPublic)
                methodInfo.Invoke(obj, {Me})
            End If
        End Sub
    End Class
End Namespace
