﻿Imports MyIdealProp.Workflow.Client

Public Class TasklistControl
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindTask()
    End Sub

    Public Sub BindTask()
        gridTasks.DataSource = WorkflowService.GetMyWorklist()
        gridTasks.DataBind()

        If Not Page.IsPostBack Then
            gridTasks.GroupBy(gridTasks.Columns("ProcSchemeDisplayName"))
        End If
    End Sub

    Public Function GetMarkColor(priority As Integer)
        Dim colors As New Dictionary(Of Integer, String)
        colors.Add(0, "gray")
        colors.Add(1, "#a820e1")
        colors.Add(2, "#ec471b")
        colors.Add(3, "#7bb71b")
        Dim color = colors.Item(priority)
        If (color Is Nothing) Then
            Throw New Exception("Can't find color " & priority & "In GetMarkColor")
        End If
        Return color
    End Function
End Class