Public Class ControlPanelForm
    Private Sub ControlPanelForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' قم بملء قائمة العملات هنا
        CheckedListBox1.Items.AddRange(New String() {"Bitcoin", "Ethereum", "Ripple", "Cardano", "Polkadot", "Solana", "Binance Coin", "Dogecoin", "Shiba Inu", "Litecoin", "Chainlink", "Stellar", "USD Coin", "Uniswap", "Bitcoin Cash", "VeChain", "Aave", "Filecoin", "TRON"})

        ' تحميل العملات المفضلة من الإعدادات
        Dim selectedCurrencies = My.Settings.SelectedCurrencies.Split(","c).ToList()
        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, selectedCurrencies.Contains(CheckedListBox1.Items(i).ToString()))
        Next
    End Sub

    Private Sub ButtonSave_Click(sender As Object, e As EventArgs) Handles ButtonSave.Click
        ' حفظ العملات المفضلة
        Dim selectedCurrencies As New List(Of String)
        For Each item In CheckedListBox1.CheckedItems
            selectedCurrencies.Add(item.ToString())
        Next
        My.Settings.SelectedCurrencies = String.Join(",", selectedCurrencies)
        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub
End Class
