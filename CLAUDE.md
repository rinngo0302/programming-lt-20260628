# プロジェクトの記録 (CHANGELOG)

プロジェクトが大きくなるにつれて経緯が埋もれていくため、進捗・決定事項・発見した不具合は [docs/CHANGELOG.md](docs/CHANGELOG.md) に時系列で残す。

- 各PRをマージしたタイミングで追記する(ブランチ運用セクション参照)
- 「何を実装したか」だけでなく「なぜそうしたか」「何が問題だったか」を書く(コミットログだけでは追いにくい背景・原因を残す)
- 仕様自体の変更・追加は `docs/CHANGELOG.md` ではなく `docs/spec/` 配下を更新する(CHANGELOGは経緯のログ、specは現在の仕様)

# セットアップ

リポジトリをクローンしたら最初に1回だけ実行:

```bash
git config core.hooksPath .githooks
dotnet tool restore
```

これでコミット時に staged な `.cs` ファイルが CSharpier で自動整形される(pre-commitフック)。

Unity MCP(Unity_ManageEditor等)を使う場合は **Unity Editorを先に起動してからClaude Codeを起動する** こと。逆順だとMCP接続に失敗し、そのセッション中は再接続されない(Unity Editorを後から起動してもツールが復活しない)。繋がらない場合はClaude Codeセッションを再起動する。

# コードスタイル

- フォーマッターは CSharpier ([.editorconfig](.editorconfig) / [.csharpierignore](.csharpierignore))
- 手動で整形する場合: `dotnet csharpier format .`
- 整形だけ確認する場合: `dotnet csharpier check .`

# ブランチ運用

実装フェーズ(機能実装以降)は **Issue駆動 + PR必須** で進める。1つのIssue(リーフIssue)に対して以下の手順を1サイクルとして繰り返す。

1. 対応するIssueを確認する(`docs/spec/`配下の仕様書も参照する)
2. `main` から `feature/xxx` または `fix/xxx` ブランチを作成する
3. 実装する(「実装完了時のチェック」セクションのチェックを通す)
4. `/code-review` スキルでセルフレビューを行い、指摘があれば修正する
5. `/pr-summary` スキルでPR本文を生成する(生成された本文をそのまま使う)
6. `gh pr create` でPRを作成する(対応するIssueを `Closes #N` で紐づける)
7. PRをマージし、`main`に取り込む
8. [docs/CHANGELOG.md](docs/CHANGELOG.md) に今回のPR・発見した不具合・決定事項を追記する(これも `main` 直接コミットせずブランチ+PRで行う。前段の修正PRに含めてもよい)
9. 次のIssueに進む

- `main` への直接コミットは行わない
- 1つのリーフIssue = 1PRを基本単位とする(大きくなりすぎる場合はIssueを分割する)

ドキュメント整備など実装フェーズ以前の作業は、これまでの運用(直接コミット)のままで構わない。

# コミットメッセージ

```
プレフィックス: 変更内容の要約(50字以内)

必要に応じて詳細を記述(本文は72字以内で折り返し)
```

- 言語: 日本語
- 単位: 1コミット = 1論理変更。複数の関心事を1コミットに混ぜない(フォーマット適用の`style`と実装の`feat`/`fix`は必ず分ける)

| プレフィックス | 用途 |
|---|---|
| `feat:` | 新機能実装 |
| `fix:` | バグ修正 |
| `refactor:` | リファクタリング(仕様には影響のない変更) |
| `chore:` | 雑用(ツールのインストールや単純なファイルの置き換えなど) |
| `docs:` | ドキュメントのみの変更(README、CLAUDE.md など) |
| `style:` | 動作に影響しないコードスタイルの変更(フォーマット適用など。`refactor`との違いはロジックを一切変えていないこと) |
| `test:` | テストの追加・修正 |
| `perf:` | パフォーマンス改善 |

良い例:
```
feat: プレイヤーのジャンプ処理を実装
fix: ダメージ計算でゼロ除算が発生する問題を修正
```

悪い例: `update` / `fix bug` / `WIP` / `変更`

# 命名規則

| 対象 | スタイル | 例 |
|---|---|---|
| クラス・構造体 | PascalCase | `PlayerController` |
| メソッド | PascalCase | `TakeDamage()` |
| インターフェース | `I` + PascalCase | `IDamageable` |
| public / protected フィールド | camelCase | `moveSpeed` |
| private フィールド | `_` + camelCase | `_currentHp` |
| ローカル変数・引数 | camelCase | `deltaTime` |

`.editorconfig` で private フィールドの `_camelCase` は自動チェックされる(suggestion)。

# C# スタイル

- namespace は必須にしない(プロジェクト名がスペース入りで使いづらいため省略可。グローバルスコープでよい)。
- 波括弧は常に改行して開く(`csharp_new_line_before_open_brace = all`、CSharpierが自動整形)。
- `using` は `System` 系を先頭に置く。
- フィールド・プロパティ・メソッドへの `this.` 修飾は不要(`.editorconfig`で抑制)。

# Unity固有のルール

### MonoBehaviour ライフサイクル

| メソッド | 役割 |
|---|---|
| `Awake` | コンポーネントの参照取得・初期化(他オブジェクトへの依存なし) |
| `Start` | 他コンポーネントへの依存がある初期化 |
| `Update` | フレームごとの軽量な処理のみ。重い処理・GC Allocを発生させる処理は書かない |

### コンポーネント参照のキャッシュ

`GetComponent` / `FindObjectOfType` は `Awake` / `Start` でキャッシュし、`Update`内で毎フレーム呼び出さない。

```csharp
// NG
void Update() { GetComponent<Rigidbody>().AddForce(...); }

// OK
Rigidbody _rb;
void Awake() { _rb = GetComponent<Rigidbody>(); }
void Update() { _rb.AddForce(...); }
```

### null チェック

- 通常の C# オブジェクト → `is null` を使う(パフォーマンス)
- `UnityEngine.Object` のライフサイクル確認(Destroy済み判定) → `== null` を使う

### SerializeField・インスペクター公開

```csharp
[Header("移動設定")]
[SerializeField, Tooltip("移動速度 (m/s)")]
float _moveSpeed = 5f;
```

パラメータは `[SerializeField]` で公開し、`public` フィールドは原則使わない。`[Header]` / `[Tooltip]` を付けて非エンジニアが扱いやすくする。

### Canvas作成時の注意

Unity MCPツール(`Unity_ManageGameObject` の `components_to_add`)でCanvasを追加すると、標準の「GameObject > UI > Canvas」メニュー経由と異なり `renderMode` が `WorldSpace` になる場合がある(Titleシーンで実際に発生した不具合)。Canvas作成後は `renderMode` が意図した値(通常は `ScreenSpaceOverlay`)になっているか必ず確認する。

### 非同期処理・イベント

- コルーチンと `async/await` を混在させない。コルーチンに統一する。
- イベント・値の監視には R3 を使用する(`UnityEvent` との混在禁止)。購読は必ずライフサイクル管理を行う(`AddTo(this)`)。
- Presenterへのバインドは `Xxx Output` struct を介して渡す。struct はただのデータ入れ物(Observable/ReadOnlyReactivePropertyを公開する口をまとめたもの)で、Model側の `CreateOutput()` で生成する。Presenterは MonoBehaviour の参照を直接持たない。Presenter側はその struct を引数に取る `Bind()` メソッドで各Observableを `Subscribe()` する。

```csharp
public struct ScoreOutput
{
    public ReadOnlyReactiveProperty<int> RedScore { get; init; }
}

public ScoreOutput CreateOutput() => new ScoreOutput { RedScore = _redScore };

public void Bind(ScoreOutput output)
{
    output.RedScore.Subscribe(UpdateRedScoreUI).AddTo(this);
}
```

# アーキテクチャ方針

- SOLID原則: 単一責任・開放閉鎖・依存性逆転を意識する。
- DRY原則: 重複ロジックはユーティリティや基底クラスに切り出す。
- UI: MVP(Model-View-Presenter)パターンを基本とする。
- ゲームデータ: `ScriptableObject` を積極的に活用する。
- YAGNI: 現時点で必要な機能のみ実装する。将来の拡張を見越した過剰な抽象化はしない。

# 実装完了時のチェック

機能の実装が完了したら、コミット前に以下を確認する。

1. リンター/フォーマッターチェック: `dotnet csharpier check .` でフォーマット崩れがないか確認(崩れていれば `dotnet csharpier format .` で整形)
2. ビルドチェック: Unity_ManageEditor などのMCPツールでコンパイルエラー・警告が無いことを確認する(Unity_GetConsoleLogs の errorCount/warningCount が 0 であること)
3. テスト: 実装内容にロジックが含まれる場合は対応するテストを書き、Unity Test Runner で green になることを確認する
4. 見た目の確認: シーン・UIなど視覚的な変更を行った場合、コンパイルが通るだけで完了とせず、`Unity_Camera_Capture` 等のMCPツールで実際の見た目をスクリーンショット確認する(Canvasのレンダリングモードによってはシーンにカメラが必要。Screen Space - Overlayはカメラ不要だが`Unity_Camera_Capture`では撮影できないため、確認したい場合はCanvasのrenderModeを一時的にScreen Space - Cameraにするなどで対応する)。コードレビューだけでは検出できない不具合(文字色と背景色が同化して視認できない等)はこの確認で見つかる

すべて問題なければコミットする。PRには可能な範囲でスクリーンショットを含める(`docs/screenshots/`配下に保存しPR本文にMarkdown画像として埋め込む)。

# テスト

- フレームワーク: Unity Test Framework (`com.unity.test-framework`, NUnitベース)
- EditMode用テストアセンブリ: [Assets/Tests/EditMode/EditModeTests.asmdef](Assets/Tests/EditMode/EditModeTests.asmdef)
  - 実機/Play不要なロジック(純粋なC#クラス・メソッド)のユニットテストはここに置く
- PlayModeテスト(MonoBehaviour・コルーチン等、実行時挙動の検証)が必要になったら `Assets/Tests/PlayMode/PlayModeTests.asmdef` を同様の形式で追加する(現時点では未作成)
- 実行方法: Unity Editor の `Window > General > Test Runner` から実行、または `EditMode`/`PlayMode` タブで対象を選択

## 何をテストするか

- **ビジネスロジックを優先する**。スコア計算・状態遷移・当たり判定の結果・ダメージ計算など、入力に対して出力/状態が一意に決まる純粋なロジックがテスト対象の中心。
- MonoBehaviourの薄いラッパー(他コンポーネントへの委譲だけ、UI表示の更新だけ)やUnity自体の挙動はテストしない。ロジックは `Model` / 純粋なC#クラスに分離してテスト容易性を確保する(MVPのModel層に置く)。
- DOTweenのアニメーション完了タイミングなど、Unityランタイムに強く依存する見た目の検証は対象外。

## テストの書き方

- **仕様(振る舞い)をテストする。実装の詳細をテストしない。** private なフィールド・メソッドや内部の実装手順を直接アサートしない。公開API(public メソッド・プロパティ・公開されたイベント/Observable)への入力と、その結果として観測できる出力・状態変化だけを検証する。
- 判断基準: 「内部実装(アルゴリズム、ループの書き方、ヘルパーメソッドの分割など)を変えてもテストが落ちない」「仕様(入出力の対応関係)を変えるとテストが落ちる」状態になっていること。後者が成立しない・前者で落ちるテストは実装をなぞっているだけなので書き直す。
- 1テスト1振る舞い。テスト名は `対象_条件_期待結果` のように仕様が読める名前にする(例: `TakeDamage_HpがArmorを下回る攻撃_HpはArmor分のみ減少する`)。
- Arrange-Act-Assert で構成し、Assertは可能な限り1つの観測可能な結果に絞る。
- 境界値・異常系(0、負数、null、上限超えなど)を仕様として明記されている範囲でカバーする。仕様にない振る舞いを推測してテストしない。

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
