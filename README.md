# Live-Digital-Cryptocurrency-Prices
Live Digital Cryptocurrency Prices

ملخص المشروع البرمجي: مراقبة أسعار العملات الرقمية وتنبيه المستخدمين


+ اسم المشروع: مراقب أسعار العملات الرقمية

+ الغرض من المشروع:


مشروع يهدف إلى مراقبة أسعار العملات الرقمية (مثل Bitcoin وEthereum وغيرها) وتنبيه المستخدمين عند حدوث تغييرات كبيرة في الأسعار أو عند وصول الأسعار إلى هدف محدد مسبقًا.

+ التكنولوجيا المستخدمة:

لغة البرمجة: Visual Basic .NET


+ مكتبات:


System.Net.Http للوصول إلى واجهة برمجة التطبيقات (API) الخاصة بالأسعار.
Newtonsoft.Json لتحليل بيانات JSON.
System.Net.Mail لإرسال الإخطارات عبر البريد الإلكتروني.
Twilio لإرسال الإخطارات عبر الرسائل النصية (SMS).


+ المزايا الرئيسية:

جلب البيانات: جلب أسعار العملات الرقمية من واجهة برمجة التطبيقات لـ CoinGecko.
التحديث الدوري: تحديث الأسعار بشكل دوري يعتمد على فترة زمنية يحددها المستخدم.
إخطارات التغيير: تنبيه المستخدمين عند حدوث تغييرات كبيرة في الأسعار (مثل زيادة أو انخفاض بنسبة معينة).
إخطارات الأهداف: تنبيه المستخدمين عند وصول الأسعار إلى أهداف محددة مسبقًا.
الإخطارات المتعددة: إرسال إخطارات عبر البريد الإلكتروني والرسائل النصية (SMS).
واجهة مستخدم مرنة: واجهة مستخدم تمكن المستخدم من تخصيص العملات المراد مراقبتها والأهداف السعرية.


+ التحديات التي تم مواجهتها:

التعامل مع الأخطاء: التعامل مع الأخطاء الممكنة أثناء جلب البيانات من الإنترنت أو إرسال الإخطارات، وتسجيل هذه الأخطاء في ملف سجل.
التحديث الدوري: ضمان التحديث الدوري السلس للأسعار دون التأثير على أداء التطبيق.


+ كيفية استخدام المشروع:

إعداد التطبيق: تشغيل التطبيق وضبط فترة التحديث الدوري من خلال التحكم في واجهة المستخدم.
تحديد العملات: اختيار العملات المراد مراقبتها وإدخال الأهداف السعرية إذا كان ذلك مطلوبًا.
تفعيل الإخطارات: تحديد طرق الإخطار (البريد الإلكتروني والرسائل النصية) وتفعيلها.
بدء التحديث: الضغط على زر بدء التحديث لبدء عملية مراقبة الأسعار وتحديثها بشكل دوري.


+ ملاحظات إضافية:

يجب على المستخدمين توفير بيانات المصادقة الخاصة بخدمة Twilio لإرسال الرسائل النصية.
يجب على المستخدمين إعداد خادم SMTP صحيح لإرسال البريد الإلكتروني.
هذا التلخيص يوضح بشكل شامل الغرض من المشروع، المزايا التي يقدمها، التكنولوجيا المستخدمة، التحديات التي تم التغلب عليها، وكيفية استخدامه بشكل فعال.



-- --

Software project summary: Monitor cryptocurrency prices and alert users


+ Project name: Digital currency price monitor

+ Purpose of the project:


A project that aims to monitor the prices of digital currencies (such as Bitcoin, Ethereum, etc.) and alert users when significant price changes occur or when prices reach a pre-defined target.

+ Technology used:

Programming language: Visual Basic .NET


+ Libraries:


System.Net.Http to access the pricing API.
Newtonsoft.Json to parse JSON data.
System.Net.Mail to send notifications via email.
Twilio to send notifications via text message (SMS).


+ Main advantages:

Fetch data: Fetch cryptocurrency prices from CoinGecko API.
Periodic update: Prices are updated periodically based on a time period specified by the user.
Change Notifications: Alert users when significant price changes occur (such as a certain percentage increase or decrease).
Target Notifications: Alert users when prices reach pre-set targets.
Multiple Notifications: Send notifications via email and text messages (SMS).
Flexible user interface: A user interface that enables the user to customize the currencies to be monitored and price targets.


+ Challenges faced:

Error handling: Handle possible errors while fetching data from the Internet or sending notifications, and record these errors in a log file.
Periodic update: Ensure smooth periodic update of prices without affecting the application performance.


+ How to use the project:

Application setting: Run the application and set the periodic update period by controlling the user interface.
Select currencies: Choose currencies to monitor and enter price targets if required.
Activate notifications: Define notification methods (email and text messages) and activate them.
Start updating: Click on the Start Update button to start the process of monitoring prices and updating them periodically.


+Additional notes:

Users must provide Twilio authentication data to send text messages.
Users must set up a correct SMTP server to send email.
This summary comprehensively explains the purpose of the project, the advantages it offers, the technology used, the challenges overcome, and how to use it effectively.

