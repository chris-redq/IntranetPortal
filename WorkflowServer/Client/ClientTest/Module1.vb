Imports MyIdealProp.Workflow.Client

Module Module1

    Sub Main()
        Dim i = 0
        Try
            Dim j = 1
            'While True
            '    Using conn As New Connection("localhost")
            '        conn.UserName = "Tester"
            '        conn.Open()
            '        Dim procInst = conn.CreateProcessInstance("TaskProcess")
            '        Dim wl = conn.OpenMyWorklist()
            '    End Using

            '    i = i + 1
            '    If i Mod 100 = 0 Then
            '        Console.WriteLine(j)
            '        j = j + 1
            '    End If
            'End While

            Using conn As New Connection("localhost")
                conn.UserName = "Jay Gottlieb"
                conn.Open()
                Dim wl = conn.OpenMyWorklist()
            End Using
        Catch ex As Exception
            Console.WriteLine("Exception: " & ex.Message & ", i= " & i)
        End Try

    End Sub
End Module
