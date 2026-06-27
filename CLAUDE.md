# セットアップ

リポジトリをクローンしたら最初に1回だけ実行:

```bash
git config core.hooksPath .githooks
dotnet tool restore
```

これでコミット時に staged な `.cs` ファイルが CSharpier で自動整形される(pre-commitフック)。

# コードスタイル

- フォーマッターは CSharpier ([.editorconfig](.editorconfig) / [.csharpierignore](.csharpierignore))
- 手動で整形する場合: `dotnet csharpier format .`
- 整形だけ確認する場合: `dotnet csharpier check .`
