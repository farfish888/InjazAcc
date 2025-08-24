# InjazAcc — نظام محاسبي للحراسات (WPF .NET 8)

**جاهز للرفع على GitHub**: مشروع WPF (MVVM) + EF Core + SQLite مع وحدات أساسية:
- الفواتير (VAT 15% افتراضيًا) + ترحيل قيد يومية تلقائي.
- ميزان مراجعة مجمّع.
- نواة ZATCA (QR TLV + UBL XML مبسّط placeholder).
- دليل حسابات مبدئي يشمل 3200 جاري المالك.
- RTL وخط Cairo/Segoe UI وثيم أخضر/تركواز.

## البدء السريع
1) افتح مجلد `InjazAcc` في VS Code أو Visual Studio.
2) استرجاع الحزم:
   ```bash
   dotnet restore
   ```
3) البناء والتشغيل:
   ```bash
   dotnet build
   dotnet run --project src/InjazAcc.UI/InjazAcc.UI.csproj
   ```
4) تسجيل الدخول:
   - اسم المستخدم: `admin`
   - كلمة المرور: `1234`

> قاعدة البيانات: ملف `injazacc.db` بجانب التنفيذ. يتم **إنشاء/تهيئة** الدليل والمستخدمين تلقائيًا.

## البنية
```
src/
 ├─ InjazAcc.UI           (WPF — Views/VMs/Styles)
 ├─ InjazAcc.Domain       (Entities)
 ├─ InjazAcc.Infrastructure (EF Core DbContext + Seeding + تقارير)
 ├─ InjazAcc.Services     (Auth/Posting/VAT/ZATCA/Reports stubs)
 └─ InjazAcc.Shared       (Enums/Result/Settings)
```

## الحزم الأساسية
- CommunityToolkit.Mvvm (MVVM)
- Microsoft.EntityFrameworkCore.Sqlite (قاعدة SQLite للتطوير)
- QRCoder / QuestPDF / ClosedXML (للتوسعة لاحقًا)

## ملاحظات ZATCA
- الـ QR بصيغة TLV **غير موقَّع** (لأغراض التطوير). عند الانتقال للإنتاج اربط APIs لشهادات وحلول توقيع حسب دليل ZATCA.
- UBL XML هنا **مبسّط**، غرضه وضع المسار فقط.

## أفكار GitHub
- **Branches**: `main` (مستقر)، `dev` (دمج التطوير)، `feature/*` للميزات.
- **Issues**: افتح تذاكر لكل ميزة/خطأ، اربطها بـ Pull Requests.
- **Projects (Kanban)**: To Do / In Progress / Review / Done.
- **Actions**: أضف Workflow البناء على Windows لرفع Artifact.
- **Releases/Tags**: استخدم `v0.1.0`، `v0.2.0`... مع ملاحظات إصدار.
- **Codeowners/PR Rules**: حماية فرع `main` وفرض مراجعة واحد على الأقل قبل الدمج.
- **Templates**: أضف `PULL_REQUEST_TEMPLATE.md` و`ISSUE_TEMPLATE.md` لاحقًا.

## الخطوات التالية
- إضافة وحدات: التحصيلات، المصروفات، الأصول، الرواتب، المطابقة البنكية.
- توسيع التقارير: قائمة الدخل والميزانية، VAT (مخرجات/مدخلات).
- ترقيم سلاسل المستندات وسياسات الصلاحيات وسجل التدقيق.
