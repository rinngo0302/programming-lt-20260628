# Changelog

プロジェクトの進捗・決定事項・発見した不具合などを時系列で記録する。
詳細な仕様は [docs/spec/](spec) を、開発ルールは [CLAUDE.md](../CLAUDE.md) を参照。

## 2026-06-27

- chore: CSharpier + .editorconfig を導入([c2986f2](https://github.com/rinngo0302/programming-lt-20260628/commit/c2986f2))
- chore: pre-commitフックでCSharpier自動整形を導入([981c06e](https://github.com/rinngo0302/programming-lt-20260628/commit/981c06e))
- test: EditModeテストの土台を追加([bf90d54](https://github.com/rinngo0302/programming-lt-20260628/commit/bf90d54))
- docs: マリオカート風レースゲームのv1仕様書を追加([d478181](https://github.com/rinngo0302/programming-lt-20260628/commit/d478181)) — [docs/spec/](spec)
- chore: Issueテンプレートを追加、実装フェーズはIssue駆動+PR必須の運用に変更
- v1実装タスクをIssue化(親 [#1](https://github.com/rinngo0302/programming-lt-20260628/issues/1) + scope親9件 + リーフ約40件、3階層のsub-issue構成)
- [PR #53](https://github.com/rinngo0302/programming-lt-20260628/pull/53) feat(scene): タイトルシーンを作成する([#13](https://github.com/rinngo0302/programming-lt-20260628/issues/13))
  - 日本語表示のためNoto Sans JP(OFL)を導入、TMP Font Assetを生成
- [PR #54](https://github.com/rinngo0302/programming-lt-20260628/pull/54) fix: 日本語TMPフォントのアトラステクスチャ欠落を修正
  - 原因: `TMP_FontAsset.CreateFontAsset`生成時にアトラス/マテリアルをサブアセット登録していなかった
- [PR #55](https://github.com/rinngo0302/programming-lt-20260628/pull/55) chore: Git LFS用フックを追加
- [PR #56](https://github.com/rinngo0302/programming-lt-20260628/pull/56) fix: `.githooks/`配下の全フックに実行権限を付与
  - 発見: `pre-commit`(CSharpier整形)を含む全フックが実行権限なし(644)でコミットされており、クローン環境で機能しない不具合があった
- [PR #57](https://github.com/rinngo0302/programming-lt-20260628/pull/57) docs: CHANGELOGを新設し、これまでの進捗を記録する
- [PR #58](https://github.com/rinngo0302/programming-lt-20260628/pull/58) feat(scene): ゲームシーンの土台を作成する([#14](https://github.com/rinngo0302/programming-lt-20260628/issues/14))
  - 発見: `Title.unity`(#13で作成)がBuild Settingsに未登録だったため、同PRでまとめて登録
- [PR #59](https://github.com/rinngo0302/programming-lt-20260628/pull/59) docs: CHANGELOGにPR #57/#58を追記
- [PR #60](https://github.com/rinngo0302/programming-lt-20260628/pull/60) feat(scene): リザルトシーンを作成する([#15](https://github.com/rinngo0302/programming-lt-20260628/issues/15))
  - 実装時、Unity_Camera_Captureで実際の見た目をスクリーンショット確認する運用を開始(CLAUDE.mdにも以後の方針として記録予定)
  - 発見: ボタンの子テキストが白背景に白文字で視認できない不具合をスクリーンショット確認で発見・修正(コードレビューだけでは検出できない見た目の不具合)
  - `Result.unity` をBuild Settingsに登録
- [PR #61](https://github.com/rinngo0302/programming-lt-20260628/pull/61) docs: 実装完了時のチェックにスクリーンショット確認を追加
- [PR #62](https://github.com/rinngo0302/programming-lt-20260628/pull/62) feat(scene): シーン遷移ロジックを実装する([#16](https://github.com/rinngo0302/programming-lt-20260628/issues/16))
  - `SceneLoader`(SceneManager.LoadSceneの薄いラッパー) + `TitleScenePresenter`/`ResultScenePresenter`でボタンを接続
  - Play modeでボタンクリックをシミュレートし、3パターンの遷移を実機相当で確認
  - scope親 [#2 シーン](https://github.com/rinngo0302/programming-lt-20260628/issues/2) のリーフ4件(#13〜#16)がすべて完了したためクローズ
- [PR #63](https://github.com/rinngo0302/programming-lt-20260628/pull/63) docs: CHANGELOGにPR #61/#62を追記
- MVPのBindings structをOutput命名に変更([PR #64](https://github.com/rinngo0302/programming-lt-20260628/pull/64))
  - struct名: `XxxBindings` → `XxxOutput`、生成メソッド名: `CreateBindings()` → `CreateOutput()`(Presenterの`Bind()`はそのまま)
- [PR #65](https://github.com/rinngo0302/programming-lt-20260628/pull/65) chore(input): 不要なInput Actionsを削除する([#17](https://github.com/rinngo0302/programming-lt-20260628/issues/17))
  - Player マップから Attack/Interact/Crouch/Previous/Next を削除。Move/Look/Sprint/Jump(#18で再定義予定)とUIマップは維持
- [PR #66](https://github.com/rinngo0302/programming-lt-20260628/pull/66) docs: CHANGELOGにPR #63/#64/#65を追記
- [PR #67](https://github.com/rinngo0302/programming-lt-20260628/pull/67) feat(input): ステアリング/加速/ブレーキのActionを定義する([#18](https://github.com/rinngo0302/programming-lt-20260628/issues/18))
  - Player マップの `Move`(Vector2) を廃止し、`Steer`(Axis, A/D・←→の1DAxis合成)・`Accelerate`(Button, W/↑)・`Brake`(Button, S/↓) を新規定義。キーボード操作のみに限定
  - 作業中、Unity MCPサーバーとの接続がセッション内で切れる事象が発生(Claude起動→Unity Editor起動の順序が原因。Unity起動後にClaude側を再起動して復旧)
- [PR #68](https://github.com/rinngo0302/programming-lt-20260628/pull/68) docs: CHANGELOGにPR #66/#67を追記、Unity MCP起動順序の注意点を記載
- [PR #69](https://github.com/rinngo0302/programming-lt-20260628/pull/69) fix: タイトルシーンのCanvas RenderModeがWorldSpaceになっていた不具合を修正
  - ユーザーからの指摘(「カメラがないのでは」)を起点に発見。`Unity_ManageGameObject`でCanvas追加時、標準メニュー経由と異なり`renderMode`が`WorldSpace`になっていた(#13から潜在)
  - 副次的に、タイトルテキストの折り返し崩れとスタートボタンの文字色不具合(白背景に白文字)もスクリーンショット確認で発見・修正
- [PR #70](https://github.com/rinngo0302/programming-lt-20260628/pull/70) docs: CHANGELOGにPR #68/#69を追記
- [PR #71](https://github.com/rinngo0302/programming-lt-20260628/pull/71) feat(input): アイテム使用のActionを定義する([#19](https://github.com/rinngo0302/programming-lt-20260628/issues/19))
  - `UseItem`(Button, Space)を定義。未使用だった`Jump`をSpaceキーから付け替えて再利用し、キーボード専用に統一
  - scope親 [#3 input](https://github.com/rinngo0302/programming-lt-20260628/issues/3) のリーフ3件(#17〜#19)がすべて完了したためクローズ
- [PR #72](https://github.com/rinngo0302/programming-lt-20260628/pull/72) docs: CHANGELOGにPR #70/#71を追記、scope#3完了を記録
- [PR #73](https://github.com/rinngo0302/programming-lt-20260628/pull/73) feat(course): CourseDataのScriptableObjectを定義する([#20](https://github.com/rinngo0302/programming-lt-20260628/issues/20))
  - チェックポイント・ウェイポイント・アイテムボックス位置(Vector3[])とラップ数を保持するシンプルなデータコンテナを実装
- [PR #74](https://github.com/rinngo0302/programming-lt-20260628/pull/74) docs: CHANGELOGにPR #72/#73を追記
- [PR #75](https://github.com/rinngo0302/programming-lt-20260628/pull/75) feat(course): チェックポイントトリガーを実装する([#21](https://github.com/rinngo0302/programming-lt-20260628/issues/21))
  - `Checkpoint`(順番付き、トリガー+R3 Observableで通過通知)と、シーン配置をCourseDataへ同期する`CourseDefinition`を実装
  - Play modeで実際にトリガー通過を確認
- [PR #76](https://github.com/rinngo0302/programming-lt-20260628/pull/76) chore: `.idea/`をgitignoreに追加
