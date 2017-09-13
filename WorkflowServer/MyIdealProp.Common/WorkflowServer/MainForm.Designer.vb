<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim GridLevelNode1 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Dim GridLevelNode2 As DevExpress.XtraGrid.GridLevelNode = New DevExpress.XtraGrid.GridLevelNode()
        Me.gridActInst = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn10 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn11 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn12 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn13 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn14 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn15 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn16 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridProcHistory = New DevExpress.XtraGrid.GridControl()
        Me.gridEventInst = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn17 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn18 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn19 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn20 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.gridProcInst = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn5 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn6 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn7 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn8 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn9 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn21 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn22 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.btnGetAllWorklist = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnRetry = New System.Windows.Forms.Button()
        Me.btnAllErrors = New System.Windows.Forms.Button()
        Me.txtLoginUser = New System.Windows.Forms.TextBox()
        Me.btnDatafields = New System.Windows.Forms.Button()
        Me.txtProcessName = New System.Windows.Forms.TextBox()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.btnComplete = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.gridWorklist = New DevExpress.XtraGrid.GridControl()
        Me.GridView1 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.gridProcess = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn1 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn2 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn3 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn4 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.RepositoryItemButtonEdit1 = New DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.gridErrors = New DevExpress.XtraGrid.GridControl()
        Me.GridView2 = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.GridColumn23 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn24 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn25 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn26 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn27 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.GridColumn28 = New DevExpress.XtraGrid.Columns.GridColumn()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.btnDeploy = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNewProcScheme = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtNewProcDescription = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtNewProcDisplayName = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNewProcName = New System.Windows.Forms.TextBox()
        CType(Me.gridActInst, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridProcHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridEventInst, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.gridProcInst, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.gridWorklist, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage2.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        CType(Me.gridErrors, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TabPage4.SuspendLayout()
        Me.SuspendLayout()
        '
        'gridActInst
        '
        Me.gridActInst.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn10, Me.GridColumn11, Me.GridColumn12, Me.GridColumn13, Me.GridColumn14, Me.GridColumn15, Me.GridColumn16})
        Me.gridActInst.GridControl = Me.gridProcHistory
        Me.gridActInst.Name = "gridActInst"
        Me.gridActInst.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never
        '
        'GridColumn10
        '
        Me.GridColumn10.Caption = "ActInstId"
        Me.GridColumn10.FieldName = "ActInstId"
        Me.GridColumn10.Name = "GridColumn10"
        Me.GridColumn10.Visible = True
        Me.GridColumn10.VisibleIndex = 0
        '
        'GridColumn11
        '
        Me.GridColumn11.Caption = "Destination User"
        Me.GridColumn11.FieldName = "DestinationUser"
        Me.GridColumn11.Name = "GridColumn11"
        Me.GridColumn11.Visible = True
        Me.GridColumn11.VisibleIndex = 1
        '
        'GridColumn12
        '
        Me.GridColumn12.Caption = "Activity Name"
        Me.GridColumn12.FieldName = "ActivityName"
        Me.GridColumn12.Name = "GridColumn12"
        Me.GridColumn12.Visible = True
        Me.GridColumn12.VisibleIndex = 2
        '
        'GridColumn13
        '
        Me.GridColumn13.Caption = "Start Date"
        Me.GridColumn13.FieldName = "StartDate"
        Me.GridColumn13.Name = "GridColumn13"
        Me.GridColumn13.Visible = True
        Me.GridColumn13.VisibleIndex = 3
        '
        'GridColumn14
        '
        Me.GridColumn14.Caption = "End Date"
        Me.GridColumn14.FieldName = "EndDate"
        Me.GridColumn14.Name = "GridColumn14"
        Me.GridColumn14.Visible = True
        Me.GridColumn14.VisibleIndex = 4
        '
        'GridColumn15
        '
        Me.GridColumn15.Caption = "Is Client"
        Me.GridColumn15.FieldName = "IsClient"
        Me.GridColumn15.Name = "GridColumn15"
        Me.GridColumn15.Visible = True
        Me.GridColumn15.VisibleIndex = 5
        '
        'GridColumn16
        '
        Me.GridColumn16.Caption = "Status"
        Me.GridColumn16.Name = "GridColumn16"
        Me.GridColumn16.Visible = True
        Me.GridColumn16.VisibleIndex = 6
        '
        'gridProcHistory
        '
        Me.gridProcHistory.Dock = System.Windows.Forms.DockStyle.Fill
        GridLevelNode1.LevelTemplate = Me.gridActInst
        GridLevelNode1.RelationName = "Level1"
        GridLevelNode2.LevelTemplate = Me.gridEventInst
        GridLevelNode2.RelationName = "Level2"
        Me.gridProcHistory.LevelTree.Nodes.AddRange(New DevExpress.XtraGrid.GridLevelNode() {GridLevelNode1, GridLevelNode2})
        Me.gridProcHistory.Location = New System.Drawing.Point(3, 3)
        Me.gridProcHistory.MainView = Me.gridProcInst
        Me.gridProcHistory.Name = "gridProcHistory"
        Me.gridProcHistory.Size = New System.Drawing.Size(669, 173)
        Me.gridProcHistory.TabIndex = 0
        Me.gridProcHistory.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.gridEventInst, Me.gridProcInst, Me.gridActInst})
        '
        'gridEventInst
        '
        Me.gridEventInst.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn17, Me.GridColumn18, Me.GridColumn19, Me.GridColumn20})
        Me.gridEventInst.GridControl = Me.gridProcHistory
        Me.gridEventInst.Name = "gridEventInst"
        '
        'GridColumn17
        '
        Me.GridColumn17.Caption = "Event Name"
        Me.GridColumn17.FieldName = "EventName"
        Me.GridColumn17.Name = "GridColumn17"
        Me.GridColumn17.Visible = True
        Me.GridColumn17.VisibleIndex = 0
        '
        'GridColumn18
        '
        Me.GridColumn18.Caption = "Start Date"
        Me.GridColumn18.FieldName = "StartDate"
        Me.GridColumn18.Name = "GridColumn18"
        Me.GridColumn18.Visible = True
        Me.GridColumn18.VisibleIndex = 1
        '
        'GridColumn19
        '
        Me.GridColumn19.Caption = "End Date"
        Me.GridColumn19.FieldName = "EndDate"
        Me.GridColumn19.Name = "GridColumn19"
        Me.GridColumn19.Visible = True
        Me.GridColumn19.VisibleIndex = 2
        '
        'GridColumn20
        '
        Me.GridColumn20.Caption = "Is Client"
        Me.GridColumn20.FieldName = "IsClient"
        Me.GridColumn20.Name = "GridColumn20"
        Me.GridColumn20.Visible = True
        Me.GridColumn20.VisibleIndex = 3
        '
        'gridProcInst
        '
        Me.gridProcInst.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn5, Me.GridColumn6, Me.GridColumn7, Me.GridColumn8, Me.GridColumn9, Me.GridColumn21, Me.GridColumn22})
        Me.gridProcInst.GridControl = Me.gridProcHistory
        Me.gridProcInst.Name = "gridProcInst"
        '
        'GridColumn5
        '
        Me.GridColumn5.Caption = "ProcessId"
        Me.GridColumn5.FieldName = "ProcessId"
        Me.GridColumn5.Name = "GridColumn5"
        Me.GridColumn5.Visible = True
        Me.GridColumn5.VisibleIndex = 0
        '
        'GridColumn6
        '
        Me.GridColumn6.Caption = "Originator"
        Me.GridColumn6.FieldName = "Originator"
        Me.GridColumn6.Name = "GridColumn6"
        Me.GridColumn6.Visible = True
        Me.GridColumn6.VisibleIndex = 2
        '
        'GridColumn7
        '
        Me.GridColumn7.Caption = "Display Name"
        Me.GridColumn7.FieldName = "DisplayName"
        Me.GridColumn7.Name = "GridColumn7"
        Me.GridColumn7.Visible = True
        Me.GridColumn7.VisibleIndex = 1
        '
        'GridColumn8
        '
        Me.GridColumn8.Caption = "Process Name"
        Me.GridColumn8.FieldName = "ProcessName"
        Me.GridColumn8.Name = "GridColumn8"
        Me.GridColumn8.Visible = True
        Me.GridColumn8.VisibleIndex = 3
        '
        'GridColumn9
        '
        Me.GridColumn9.Caption = "Start Date"
        Me.GridColumn9.DisplayFormat.FormatString = "G"
        Me.GridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.GridColumn9.FieldName = "StartDate"
        Me.GridColumn9.Name = "GridColumn9"
        Me.GridColumn9.Visible = True
        Me.GridColumn9.VisibleIndex = 4
        '
        'GridColumn21
        '
        Me.GridColumn21.Caption = "End Date"
        Me.GridColumn21.DisplayFormat.FormatString = "G"
        Me.GridColumn21.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime
        Me.GridColumn21.FieldName = "EndDate"
        Me.GridColumn21.Name = "GridColumn21"
        Me.GridColumn21.Visible = True
        Me.GridColumn21.VisibleIndex = 5
        '
        'GridColumn22
        '
        Me.GridColumn22.Caption = "Status"
        Me.GridColumn22.FieldName = "Status"
        Me.GridColumn22.Name = "GridColumn22"
        Me.GridColumn22.Visible = True
        Me.GridColumn22.VisibleIndex = 6
        '
        'btnCreate
        '
        Me.btnCreate.Location = New System.Drawing.Point(181, 5)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(111, 23)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Create Process"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnGetAllWorklist
        '
        Me.btnGetAllWorklist.Location = New System.Drawing.Point(181, 34)
        Me.btnGetAllWorklist.Name = "btnGetAllWorklist"
        Me.btnGetAllWorklist.Size = New System.Drawing.Size(99, 23)
        Me.btnGetAllWorklist.TabIndex = 1
        Me.btnGetAllWorklist.Text = "Worklist"
        Me.btnGetAllWorklist.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnRetry)
        Me.Panel1.Controls.Add(Me.btnAllErrors)
        Me.Panel1.Controls.Add(Me.txtLoginUser)
        Me.Panel1.Controls.Add(Me.btnDatafields)
        Me.Panel1.Controls.Add(Me.txtProcessName)
        Me.Panel1.Controls.Add(Me.btnRefresh)
        Me.Panel1.Controls.Add(Me.btnComplete)
        Me.Panel1.Controls.Add(Me.btnCreate)
        Me.Panel1.Controls.Add(Me.btnGetAllWorklist)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(683, 60)
        Me.Panel1.TabIndex = 2
        '
        'btnRetry
        '
        Me.btnRetry.Location = New System.Drawing.Point(553, 34)
        Me.btnRetry.Name = "btnRetry"
        Me.btnRetry.Size = New System.Drawing.Size(75, 23)
        Me.btnRetry.TabIndex = 8
        Me.btnRetry.Text = "Retry Error"
        Me.btnRetry.UseVisualStyleBackColor = True
        '
        'btnAllErrors
        '
        Me.btnAllErrors.Location = New System.Drawing.Point(472, 34)
        Me.btnAllErrors.Name = "btnAllErrors"
        Me.btnAllErrors.Size = New System.Drawing.Size(75, 23)
        Me.btnAllErrors.TabIndex = 7
        Me.btnAllErrors.Text = "Get Errors"
        Me.btnAllErrors.UseVisualStyleBackColor = True
        '
        'txtLoginUser
        '
        Me.txtLoginUser.Location = New System.Drawing.Point(13, 36)
        Me.txtLoginUser.Name = "txtLoginUser"
        Me.txtLoginUser.Size = New System.Drawing.Size(162, 20)
        Me.txtLoginUser.TabIndex = 6
        '
        'btnDatafields
        '
        Me.btnDatafields.Location = New System.Drawing.Point(298, 6)
        Me.btnDatafields.Name = "btnDatafields"
        Me.btnDatafields.Size = New System.Drawing.Size(75, 23)
        Me.btnDatafields.TabIndex = 5
        Me.btnDatafields.Text = "Data Fields"
        Me.btnDatafields.UseVisualStyleBackColor = True
        '
        'txtProcessName
        '
        Me.txtProcessName.Location = New System.Drawing.Point(13, 8)
        Me.txtProcessName.Name = "txtProcessName"
        Me.txtProcessName.Size = New System.Drawing.Size(162, 20)
        Me.txtProcessName.TabIndex = 4
        '
        'btnRefresh
        '
        Me.btnRefresh.Location = New System.Drawing.Point(391, 34)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(75, 23)
        Me.btnRefresh.TabIndex = 3
        Me.btnRefresh.Text = "Refresh"
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'btnComplete
        '
        Me.btnComplete.Location = New System.Drawing.Point(286, 34)
        Me.btnComplete.Name = "btnComplete"
        Me.btnComplete.Size = New System.Drawing.Size(99, 23)
        Me.btnComplete.TabIndex = 2
        Me.btnComplete.Text = "Complete Task"
        Me.btnComplete.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage4)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 60)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(683, 205)
        Me.TabControl1.TabIndex = 4
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.gridWorklist)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(675, 179)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Work List"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'gridWorklist
        '
        Me.gridWorklist.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridWorklist.Location = New System.Drawing.Point(3, 3)
        Me.gridWorklist.MainView = Me.GridView1
        Me.gridWorklist.Name = "gridWorklist"
        Me.gridWorklist.RepositoryItems.AddRange(New DevExpress.XtraEditors.Repository.RepositoryItem() {Me.RepositoryItemButtonEdit1})
        Me.gridWorklist.Size = New System.Drawing.Size(669, 173)
        Me.gridWorklist.TabIndex = 0
        Me.gridWorklist.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView1})
        '
        'GridView1
        '
        Me.GridView1.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.gridProcess, Me.GridColumn1, Me.GridColumn2, Me.GridColumn3, Me.GridColumn4})
        Me.GridView1.GridControl = Me.gridWorklist
        Me.GridView1.Name = "GridView1"
        '
        'gridProcess
        '
        Me.gridProcess.Caption = "Process Name"
        Me.gridProcess.FieldName = "ProcessName"
        Me.gridProcess.Name = "gridProcess"
        Me.gridProcess.Visible = True
        Me.gridProcess.VisibleIndex = 1
        '
        'GridColumn1
        '
        Me.GridColumn1.Caption = "Activity Name"
        Me.GridColumn1.FieldName = "ActivityName"
        Me.GridColumn1.Name = "GridColumn1"
        Me.GridColumn1.Visible = True
        Me.GridColumn1.VisibleIndex = 2
        '
        'GridColumn2
        '
        Me.GridColumn2.Caption = "Display Name"
        Me.GridColumn2.FieldName = "ProcessDisplayName"
        Me.GridColumn2.Name = "GridColumn2"
        Me.GridColumn2.Visible = True
        Me.GridColumn2.VisibleIndex = 0
        '
        'GridColumn3
        '
        Me.GridColumn3.Caption = "Status"
        Me.GridColumn3.FieldName = "Status"
        Me.GridColumn3.Name = "GridColumn3"
        Me.GridColumn3.Visible = True
        Me.GridColumn3.VisibleIndex = 4
        '
        'GridColumn4
        '
        Me.GridColumn4.Caption = "Destination"
        Me.GridColumn4.FieldName = "Destination"
        Me.GridColumn4.Name = "GridColumn4"
        Me.GridColumn4.Visible = True
        Me.GridColumn4.VisibleIndex = 3
        '
        'RepositoryItemButtonEdit1
        '
        Me.RepositoryItemButtonEdit1.AutoHeight = False
        Me.RepositoryItemButtonEdit1.Buttons.AddRange(New DevExpress.XtraEditors.Controls.EditorButton() {New DevExpress.XtraEditors.Controls.EditorButton()})
        Me.RepositoryItemButtonEdit1.Name = "RepositoryItemButtonEdit1"
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.gridProcHistory)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(675, 179)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Process History"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.gridErrors)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(675, 179)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Error Logs"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'gridErrors
        '
        Me.gridErrors.Cursor = System.Windows.Forms.Cursors.Default
        Me.gridErrors.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gridErrors.Location = New System.Drawing.Point(3, 3)
        Me.gridErrors.MainView = Me.GridView2
        Me.gridErrors.Name = "gridErrors"
        Me.gridErrors.Size = New System.Drawing.Size(669, 173)
        Me.gridErrors.TabIndex = 0
        Me.gridErrors.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.GridView2})
        '
        'GridView2
        '
        Me.GridView2.Columns.AddRange(New DevExpress.XtraGrid.Columns.GridColumn() {Me.GridColumn23, Me.GridColumn24, Me.GridColumn25, Me.GridColumn26, Me.GridColumn27, Me.GridColumn28})
        Me.GridView2.GridControl = Me.gridErrors
        Me.GridView2.Name = "GridView2"
        Me.GridView2.OptionsSelection.MultiSelect = True
        '
        'GridColumn23
        '
        Me.GridColumn23.Caption = "LogId"
        Me.GridColumn23.FieldName = "LogId"
        Me.GridColumn23.Name = "GridColumn23"
        Me.GridColumn23.Visible = True
        Me.GridColumn23.VisibleIndex = 0
        '
        'GridColumn24
        '
        Me.GridColumn24.Caption = "ProcInstId"
        Me.GridColumn24.FieldName = "ProcInstId"
        Me.GridColumn24.Name = "GridColumn24"
        Me.GridColumn24.Visible = True
        Me.GridColumn24.VisibleIndex = 1
        '
        'GridColumn25
        '
        Me.GridColumn25.Caption = "ActInstId"
        Me.GridColumn25.FieldName = "ActInstId"
        Me.GridColumn25.Name = "GridColumn25"
        Me.GridColumn25.Visible = True
        Me.GridColumn25.VisibleIndex = 2
        '
        'GridColumn26
        '
        Me.GridColumn26.Caption = "Source"
        Me.GridColumn26.FieldName = "Source"
        Me.GridColumn26.Name = "GridColumn26"
        Me.GridColumn26.Visible = True
        Me.GridColumn26.VisibleIndex = 3
        '
        'GridColumn27
        '
        Me.GridColumn27.Caption = "Error Msg"
        Me.GridColumn27.FieldName = "ErrorMsg"
        Me.GridColumn27.Name = "GridColumn27"
        Me.GridColumn27.Visible = True
        Me.GridColumn27.VisibleIndex = 4
        '
        'GridColumn28
        '
        Me.GridColumn28.Caption = "CreateTime"
        Me.GridColumn28.DisplayFormat.FormatString = "G"
        Me.GridColumn28.FieldName = "CreateTime"
        Me.GridColumn28.Name = "GridColumn28"
        Me.GridColumn28.Visible = True
        Me.GridColumn28.VisibleIndex = 5
        '
        'TabPage4
        '
        Me.TabPage4.Controls.Add(Me.btnDeploy)
        Me.TabPage4.Controls.Add(Me.Label4)
        Me.TabPage4.Controls.Add(Me.txtNewProcScheme)
        Me.TabPage4.Controls.Add(Me.Label3)
        Me.TabPage4.Controls.Add(Me.txtNewProcDescription)
        Me.TabPage4.Controls.Add(Me.Label2)
        Me.TabPage4.Controls.Add(Me.txtNewProcDisplayName)
        Me.TabPage4.Controls.Add(Me.Label1)
        Me.TabPage4.Controls.Add(Me.txtNewProcName)
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(675, 179)
        Me.TabPage4.TabIndex = 3
        Me.TabPage4.Text = "TabPage4"
        Me.TabPage4.UseVisualStyleBackColor = True
        '
        'btnDeploy
        '
        Me.btnDeploy.Location = New System.Drawing.Point(88, 96)
        Me.btnDeploy.Name = "btnDeploy"
        Me.btnDeploy.Size = New System.Drawing.Size(75, 23)
        Me.btnDeploy.TabIndex = 13
        Me.btnDeploy.Text = "Deploy"
        Me.btnDeploy.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(256, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(48, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Process:"
        '
        'txtNewProcScheme
        '
        Me.txtNewProcScheme.Dock = System.Windows.Forms.DockStyle.Right
        Me.txtNewProcScheme.Location = New System.Drawing.Point(310, 3)
        Me.txtNewProcScheme.Multiline = True
        Me.txtNewProcScheme.Name = "txtNewProcScheme"
        Me.txtNewProcScheme.Size = New System.Drawing.Size(362, 173)
        Me.txtNewProcScheme.TabIndex = 11
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(9, 64)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Description:"
        '
        'txtNewProcDescription
        '
        Me.txtNewProcDescription.Location = New System.Drawing.Point(88, 61)
        Me.txtNewProcDescription.Name = "txtNewProcDescription"
        Me.txtNewProcDescription.Size = New System.Drawing.Size(162, 20)
        Me.txtNewProcDescription.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 38)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Display Name:"
        '
        'txtNewProcDisplayName
        '
        Me.txtNewProcDisplayName.Location = New System.Drawing.Point(88, 35)
        Me.txtNewProcDisplayName.Name = "txtNewProcDisplayName"
        Me.txtNewProcDisplayName.Size = New System.Drawing.Size(162, 20)
        Me.txtNewProcDisplayName.TabIndex = 7
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(38, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Name:"
        '
        'txtNewProcName
        '
        Me.txtNewProcName.Location = New System.Drawing.Point(88, 9)
        Me.txtNewProcName.Name = "txtNewProcName"
        Me.txtNewProcName.Size = New System.Drawing.Size(162, 20)
        Me.txtNewProcName.TabIndex = 5
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(683, 265)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "frmMain"
        Me.Text = "Workflow Server Monitor"
        CType(Me.gridActInst, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridProcHistory, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridEventInst, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.gridProcInst, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        CType(Me.gridWorklist, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RepositoryItemButtonEdit1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage2.ResumeLayout(False)
        Me.TabPage3.ResumeLayout(False)
        CType(Me.gridErrors, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.GridView2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents btnGetAllWorklist As System.Windows.Forms.Button
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnComplete As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents gridWorklist As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView1 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents gridProcess As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn1 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn2 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn3 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn4 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents gridProcHistory As DevExpress.XtraGrid.GridControl
    Friend WithEvents gridProcInst As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents gridActInst As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents gridEventInst As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents GridColumn5 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn6 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn7 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn8 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn9 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents txtProcessName As System.Windows.Forms.TextBox
    Friend WithEvents GridColumn10 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn11 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn12 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn13 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn14 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn15 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn16 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn17 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn18 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn19 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn20 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnDatafields As System.Windows.Forms.Button
    Friend WithEvents RepositoryItemButtonEdit1 As DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit
    Friend WithEvents GridColumn21 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn22 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents txtLoginUser As System.Windows.Forms.TextBox
    Friend WithEvents btnAllErrors As System.Windows.Forms.Button
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents gridErrors As DevExpress.XtraGrid.GridControl
    Friend WithEvents GridView2 As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents GridColumn23 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn24 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn25 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn26 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn27 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents GridColumn28 As DevExpress.XtraGrid.Columns.GridColumn
    Friend WithEvents btnRetry As System.Windows.Forms.Button
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtNewProcScheme As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtNewProcDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtNewProcDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents btnDeploy As System.Windows.Forms.Button
    Friend WithEvents txtNewProcName As System.Windows.Forms.TextBox

End Class
