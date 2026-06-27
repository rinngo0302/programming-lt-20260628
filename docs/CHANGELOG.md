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
