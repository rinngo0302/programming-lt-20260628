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

# コミットメッセージ

プレフィックス + コロン + 日本語の要約。本文で背景や理由を補足する。

- `feat:` 新機能実装
- `fix:` バグ修正
- `refactor:` リファクタリング(仕様には影響のない変更)
- `chore:` 雑用(ツールのインストールや単純なファイルの置き換えなど)
- `docs:` ドキュメントのみの変更(README、CLAUDE.md など)
- `style:` 動作に影響しないコードスタイルの変更(フォーマット適用など。`refactor`との違いはロジックを一切変えていないこと)
- `test:` テストの追加・修正
- `perf:` パフォーマンス改善

1コミット1関心事を徹底し、フォーマット適用(`style`)と実装(`feat`/`fix`)は分けてコミットする。

# 実装完了時のチェック

機能の実装が完了したら、コミット前に以下を確認する。

1. リンター/フォーマッターチェック: `dotnet csharpier check .` でフォーマット崩れがないか確認(崩れていれば `dotnet csharpier format .` で整形)
2. ビルドチェック: Unity_ManageEditor などのMCPツールでコンパイルエラー・警告が無いことを確認する(Unity_GetConsoleLogs の errorCount/warningCount が 0 であること)

どちらも問題なければコミットする。

# パッケージ構成 (Packages/manifest.json)

- **UI/入力**: com.unity.inputsystem, com.unity.ugui, com.unity.modules.ui / uielements
- **レンダリング**: com.unity.render-pipelines.universal (URP)
- **その他Unity公式**: com.unity.ai.navigation, com.unity.timeline, com.unity.visualscripting, com.unity.test-framework, com.unity.collab-proxy, com.unity.ide.rider, com.unity.ide.visualstudio, com.unity.multiplayer.center, com.unity.ai.assistant
- **サードパーティ(OpenUPM/UnityNuGet経由)**:
  - `com.cysharp.r3` (R3.Unity) + `org.nuget.r3` (R3コア) — Reactive Extensions
  - scoped registry: `package.openupm.com`(com.cysharp.r3), `unitynuget-registry.openupm.com`(org.nuget.*)
- **Asset Store経由(UPM対象外、Assets/Plugins配下に直接配置)**:
  - DOTween v1.3.030 ([Assets/Plugins/Demigiant/DOTween](Assets/Plugins/Demigiant/DOTween)) — Tweenアニメーション

新しいパッケージ/アセットを追加した場合は、このセクションを更新すること。

# Assetsディレクトリ構成

- `Assets/Scenes` — シーン
- `Assets/Settings` — URP等のレンダリング設定
- `Assets/Plugins` — サードパーティ製アセット(DOTweenなど、UPM管理外のもの)
- `Assets/Resources` — Resources.Load対象のアセット(DOTweenSettings.assetなど)
- `Assets/InputSystem_Actions.inputactions` — Input System用アクションマップ
- `Assets/TutorialInfo`, `Assets/Readme.asset` — Unityテンプレート付属のREADME(削除可)
