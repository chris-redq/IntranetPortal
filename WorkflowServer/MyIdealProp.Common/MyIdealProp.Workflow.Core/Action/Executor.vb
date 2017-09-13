Imports MyIdealProp.Workflow.Core.Model
Imports System.Reflection

Namespace MyIdealProp.Workflow.Core.Action
    Public Class Executor
        Public Shared Sub ExecuteAction(action As ActionDefinition, objInst As Object)

            Dim type__1 = action.Type
            If type__1 Is Nothing Then
                Throw New InvalidOperationException()
            End If

            Dim methodInfo = type__1.GetMethod(action.MethodName)

            If methodInfo.IsStatic Then
                Dim a = methodInfo.Invoke(Nothing, {objInst})
            Else
                Dim obj = Activator.CreateInstance(type__1, BindingFlags.NonPublic)
                methodInfo.Invoke(obj, {objInst})
            End If
        End Sub
    End Class
End Namespace
