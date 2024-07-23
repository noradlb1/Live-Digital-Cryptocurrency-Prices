Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.IO
Imports System.Net.Mail ' تحتاج لاستيراد مكتبة البريد الإلكتروني
Imports Twilio
Imports Twilio.Rest.Api.V2010.Account
Imports Twilio.Types


Public Class Form1
    Private previousPrices As New Dictionary(Of String, Decimal)
    Private targetPrices As New Dictionary(Of String, Decimal)
    Private countdown As Integer
    Private percentageChangeThreshold As Decimal = 5 ' نسبة تغيير افتراضية 5%
#Region "SMS"
    Private Sub SendSms(toPhoneNumber As String, messageBody As String)
        ' بيانات المصادقة الخاصة بـ Twilio
        Dim accountSid As String = "YOUR_TWILIO_ACCOUNT_SID"
        Dim authToken As String = "YOUR_TWILIO_AUTH_TOKEN"

        ' إعداد عميل Twilio
        TwilioClient.Init(accountSid, authToken)

        ' رقم هاتف Twilio الخاص بك
        Dim fromPhoneNumber As String = "YOUR_TWILIO_PHONE_NUMBER"

        ' إرسال الرسالة
        Dim message = MessageResource.Create(
        body:=messageBody,
        from:=New PhoneNumber(fromPhoneNumber),
        to:=New PhoneNumber(toPhoneNumber)
    )

        ' تأكيد إرسال الرسالة
        MessageBox.Show($"Message sent with SID: {message.Sid}")
    End Sub
#End Region
    Private Async Sub GetCryptoPrices()
        Try
            ' تحديث حالة التطبيق للمستخدم
            LabelStatus.Text = "Fetching data..."
            ProgressBar1.Style = ProgressBarStyle.Marquee ' تعيين نمط الـ ProgressBar كـ Marquee لتوضيح أن التحديث جاري

            Dim client As New HttpClient()
            Dim response As HttpResponseMessage = Await client.GetAsync("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin,ethereum,ripple,cardano,polkadot,solana,binancecoin,dogecoin,shiba-inu,litecoin,chainlink,stellar,usd-coin,uniswap,bitcoin-cash,vechain,aave,filecoin,tron&vs_currencies=usd")
            response.EnsureSuccessStatusCode()
            Dim json As String = Await response.Content.ReadAsStringAsync()
            Dim prices As JObject = JObject.Parse(json)

            Dim selectedCurrencies = My.Settings.SelectedCurrencies.Split(","c).ToList()

            For Each currency In selectedCurrencies
                If prices.ContainsKey(currency.ToLower()) Then
                    UpdatePrice(currency, prices(currency.ToLower())("usd"))
                End If
            Next

            ' تحديث حالة التطبيق للمستخدم
            LabelStatus.Text = "Data fetched successfully."
            ProgressBar1.Style = ProgressBarStyle.Blocks ' إعادة تعيين نمط الـ ProgressBar
        Catch ex As Exception
            LogError(ex) ' تسجيل الخطأ في ملف سجل
            MessageBox.Show("Error: " & ex.Message)
            LabelStatus.Text = "Error fetching data."
            ProgressBar1.Style = ProgressBarStyle.Blocks ' إعادة تعيين نمط الـ ProgressBar
        End Try
    End Sub

    Private Sub UpdatePrice(currency As String, currentPrice As Decimal)
        If previousPrices.ContainsKey(currency) Then
            Dim previousPrice As Decimal = previousPrices(currency)
            Dim change As Decimal = ((currentPrice - previousPrice) / previousPrice) * 100

            If Math.Abs(change) >= percentageChangeThreshold Then
                Dim direction As String = If(change > 0, "increased", "decreased")
                Dim message As String = $"{currency} price {direction} by {Math.Abs(change):0.00}% from {previousPrice} to {currentPrice}."

                ' تحديث الـ Label الخاص بالعملة
                UpdateLabel(currency, message)

                ' إظهار تحذير إذا تم تفعيل الـ CheckBox
                If CheckBox1.Checked Then
                    MessageBox.Show(message)
                End If

                ' إرسال بريد إلكتروني إذا تم تفعيل الـ CheckBox
                If CheckBoxEmailNotifications.Checked Then
                    SendEmailNotification($"Price Alert for {currency}", message)
                End If
            Else
                ' عرض السعر الحالي فقط إذا لم يتجاوز التغيير النسبة المئوية المحددة
                UpdateLabel(currency, $"{currency} current price: {currentPrice}")
            End If

            ' التحقق من الأسعار المستهدفة إذا كان الخيار مفعل
            If CheckBoxEnableTarget.Checked Then
                CheckTargetPrice(currency, currentPrice)
            End If

            previousPrices(currency) = currentPrice
        Else
            previousPrices.Add(currency, currentPrice)
            UpdateLabel(currency, $"{currency} current price: {currentPrice}")

            ' تعيين السعر المستهدف إذا لم يكن موجوداً بالفعل
            SetTargetPrice(currency)
        End If
    End Sub

    Private Sub UpdateLabel(currency As String, message As String)
        Select Case currency
            Case "Bitcoin"
                LabelBitcoin.Text = message
            Case "Ethereum"
                LabelEthereum.Text = message
            Case "Ripple"
                LabelRipple.Text = message
            Case "Cardano"
                LabelCardano.Text = message
            Case "Polkadot"
                LabelPolkadot.Text = message
            Case "Solana"
                LabelSolana.Text = message
            Case "Binance Coin"
                LabelBinanceCoin.Text = message
            Case "Dogecoin"
                LabelDogecoin.Text = message
            Case "Shiba Inu"
                LabelShibaInu.Text = message
            Case "Litecoin"
                LabelLitecoin.Text = message
            Case "Chainlink"
                LabelChainlink.Text = message
            Case "Stellar"
                LabelStellar.Text = message
            Case "USD Coin"
                LabelUSDCoin.Text = message
            Case "Uniswap"
                LabelUniswap.Text = message
            Case "Bitcoin Cash"
                LabelBitcoinCash.Text = message
            Case "VeChain"
                LabelVeChain.Text = message
            Case "Aave"
                LabelAave.Text = message
            Case "Filecoin"
                LabelFilecoin.Text = message
            Case "TRON"
                LabelTRON.Text = message
        End Select
    End Sub

    Private Sub CheckTargetPrice(currency As String, currentPrice As Decimal)
        If targetPrices.ContainsKey(currency) Then
            Dim targetPrice As Decimal = targetPrices(currency)
            If currentPrice >= targetPrice Then
                Dim message As String = $"{currency} price reached the target of {targetPrice}. Current price: {currentPrice}."
                MessageBox.Show(message, "Target Reached", MessageBoxButtons.OK, MessageBoxIcon.Information)
                ' إرسال بريد إلكتروني عند الوصول إلى السعر المستهدف
                If CheckBoxEmailNotifications.Checked Then
                    SendEmailNotification($"Target Reached for {currency}", message)
                End If

                ' إظهار الرسالة عبر رسالة نصية إذا كان الخيار مفعل
                If CheckBoxSmsNotification.Checked Then
                    Dim phoneNumber As String = TextBoxPhoneNumber.Text ' تأكد من إدخال رقم الهاتف في TextBox
                    SendSms(phoneNumber, message)
                End If
                ' إزالة السعر المستهدف بعد الوصول إليه
                targetPrices.Remove(currency)
            End If
        End If
    End Sub

    Private Sub SetTargetPrice(currency As String)
        If CheckBoxEnableTarget.Checked Then
            Select Case currency
                Case "Bitcoin"
                    If CheckBoxBitcoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxBitcoin)
                Case "Ethereum"
                    If CheckBoxEthereum.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxEthereum)
                Case "Ripple"
                    If CheckBoxRipple.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxRipple)
                Case "Cardano"
                    If CheckBoxCardano.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxCardano)
                Case "Polkadot"
                    If CheckBoxPolkadot.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxPolkadot)
                Case "Solana"
                    If CheckBoxSolana.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxSolana)
                Case "Binance Coin"
                    If CheckBoxBinanceCoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxBinanceCoin)
                Case "Dogecoin"
                    If CheckBoxDogecoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxDogecoin)
                Case "Shiba Inu"
                    If CheckBoxShibaInu.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxShibaInu)
                Case "Litecoin"
                    If CheckBoxLitecoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxLitecoin)
                Case "Chainlink"
                    If CheckBoxChainlink.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxChainlink)
                Case "Stellar"
                    If CheckBoxStellar.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxStellar)
                Case "USD Coin"
                    If CheckBoxUSDCoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxUSDCoin)
                Case "Uniswap"
                    If CheckBoxUniswap.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxUniswap)
                Case "Bitcoin Cash"
                    If CheckBoxBitcoinCash.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxBitcoinCash)
                Case "VeChain"
                    If CheckBoxVeChain.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxVeChain)
                Case "Aave"
                    If CheckBoxAave.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxAave)
                Case "Filecoin"
                    If CheckBoxFilecoin.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxFilecoin)
                Case "TRON"
                    If CheckBoxTRON.Checked Then targetPrices(currency) = GetTextBoxValue(TextBoxTRON)
            End Select
        End If
    End Sub

    Private Function GetTextBoxValue(textBox As TextBox) As Decimal
        Dim targetPrice As Decimal
        Decimal.TryParse(textBox.Text, targetPrice)
        Return targetPrice
    End Function

    Private Sub SendEmailNotification(subject As String, body As String)
        Try
            Dim smtpClient As New SmtpClient("smtp.example.com") ' استبدل بعنوان خادم SMTP الخاص بك
            smtpClient.Port = 587 ' أو أي منفذ آخر مناسب
            smtpClient.Credentials = New Net.NetworkCredential("your-email@example.com", "your-email-password") ' استبدل بالبريد الإلكتروني وكلمة المرور الخاصة بك
            smtpClient.EnableSsl = True ' تمكين التشفير

            Dim mailMessage As New MailMessage()
            mailMessage.From = New MailAddress("your-email@example.com")
            mailMessage.To.Add("recipient@example.com") ' استبدل بعنوان البريد الإلكتروني للمتلقي
            mailMessage.Subject = subject
            mailMessage.Body = body

            smtpClient.Send(mailMessage)
        Catch ex As Exception
            LogError(ex) ' تسجيل الخطأ في ملف سجل إذا فشل الإرسال
            MessageBox.Show("Failed to send email notification: " & ex.Message)
        End Try
    End Sub

    Private Sub LogError(ex As Exception)
        Dim logFilePath As String = "error_log.txt"
        Using writer As New StreamWriter(logFilePath, True)
            writer.WriteLine($"{DateTime.Now}: {ex.Message}")
            writer.WriteLine(ex.StackTrace)
            writer.WriteLine()
        End Using
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        GetCryptoPrices()
        countdown = CInt(NumericUpDown1.Value) ' إعادة تعيين العد التنازلي
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        If countdown > 0 Then
            countdown -= 1
            LabelCountdown.Text = $"Next update in: {countdown} seconds"
        Else
            LabelCountdown.Text = "Updating..."
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Interval = CInt(NumericUpDown1.Value) * 1000 ' تحويل القيمة إلى مللي ثانية
        Timer1.Start()
        Timer2.Start() ' بدء العد التنازلي
        countdown = CInt(NumericUpDown1.Value) ' تعيين العد التنازلي بناءً على القيمة المدخلة
        LabelStatus.Text = "Fetching data every " & NumericUpDown1.Value.ToString() & " seconds..."
        LabelCountdown.Text = $"Next update in: {countdown} seconds"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer1.Stop()
        Timer2.Stop()
        LabelStatus.Text = "Fetching stopped."
        LabelCountdown.Text = ""
        ProgressBar1.Style = ProgressBarStyle.Blocks ' إعادة تعيين نمط الـ ProgressBar
    End Sub
    Private Sub ButtonControlPanel_Click(sender As Object, e As EventArgs) Handles ButtonControlPanel.Click
        Dim controlPanel As New ControlPanelForm()
        controlPanel.ShowDialog()
        ' تحديث الأسعار بناءً على الإعدادات الجديدة
        GetCryptoPrices()
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' تأكد من إعداد Timer بشكل صحيح
        Timer1.Interval = 60000 ' 1 دقيقة افتراضيًا
        NumericUpDown1.Value = 60 ' 60 ثانية افتراضيًا
        LabelStatus.Text = "Ready to fetch data."
        countdown = CInt(NumericUpDown1.Value)
        LabelCountdown.Text = ""
        ProgressBar1.Style = ProgressBarStyle.Blocks ' تعيين نمط الـ ProgressBar الافتراضي
    End Sub
End Class
