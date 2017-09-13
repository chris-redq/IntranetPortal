Imports MyIdealProp.Workflow.Core
Imports MyIdealProp.Workflow.Core.Runtime
Imports MyIdealProp.Workflow.Core.Builder
Imports MyIdealProp.Workflow.Core.Parser
Imports MyIdealProp.Workflow.Core.Model

Module Module1
    Dim runTime As Runtime.WorkflowRuntime

    Sub Main()
        'Dim t = GetType(ProcessRules)
        'Dim fullName = t.FullName
        'Console.WriteLine(t.AssemblyQualifiedName)

        'Dim myType1 = Type.GetType(fullName)
        'Console.WriteLine("The full name is {0}.", myType1.FullName)

        'Dim methodInfo = t.GetMethod("LineRule")

        'If methodInfo.IsStatic Then
        '    Dim a = methodInfo.Invoke(Nothing, Nothing)
        'Else

        'End If
        'Return   
        runTime = WorkflowServer.GetInstance
        CreateTimer()
        'runTime = New Runtime.WorkflowRuntime(Guid.NewGuid)
        'WorkflowRuntimeConfigurationExtension.WithBus(runTime, New Bus.NullBus())
        'runTime.Start()

        'ServerConsole()
        'runTime.CreateInstance("TestProcess", Guid.NewGuid())

        'Console.ReadLine()
    End Sub

    Public Sub CreateTimer()
        Dim wfDb As New Data.WorkflowDatabase
        Dim i = 0
        Try
            Dim j = 1
            While True
                Dim item = wfDb.GetProcInstToExecutor()
                i = i + 1
                If i Mod 100 = 0 Then
                    Console.WriteLine(j)
                    j = j + 1
                End If
            End While
        Catch ex As Exception
            Console.WriteLine("Exception: " & ex.Message & ", i= " & i)
        End Try
        Return
    End Sub

    Public Sub ServerConsole()
        Dim result = Console.ReadLine()
        Select Case result
            Case "Create"
                'runTime.CreateInstance("TestProcess", Guid.NewGuid)
            Case "Worklist"
                Dim worklist = runTime.GetWorklist("123")
                Console.WriteLine(worklist.Count)
                For Each item In worklist
                    Console.WriteLine("Process Instance: {0}, Status: {1}", item.ProcessInstance.ProcessId, item.Status)
                Next

            Case "Complete"
                'runTime.CompleteWorklist("123")
        End Select

        ServerConsole()
    End Sub


End Module
