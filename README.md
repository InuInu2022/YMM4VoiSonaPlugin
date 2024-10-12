# YMM4VoiSonaPlugin

## これはなに？

YMM4のボイスとして **「VoiSona Talk」(ボイソナトーク)** を使えるようにしたプラグインです。

現在開発中のため、台詞に細かいパラメータを指定できません。
台詞の合成にやや時間が掛かります。
また、たまに失敗することがあります。

## インストール方法

[Releases](https://github.com/InuInu2022/YMM4VoiSonaPlugin/releases) 以下にある最新のバージョンの`YMM4VoiSonaPlugin.ymme`をインストールしてください。

「キャラクター設定」の「ボイス」で、「YMM4 VoiSona Talkプラグインの声質を再読み込み」を選択して、現在のボイスライブラリを取得してください。
※新しくボイスライブラリを増やすたびに必要です

## 使い方

1. プラグインをインストールする
2. VoiSona Talkを起動する
3. （初回）キャラクター設定で「YMM4 VoiSona Talkプラグインの声質を再読み込み」
4. キャラクターを作る
5. 作ったキャラクターを選んだ状態でセリフを入力する

### 注意点

- VoiSona Talkは起動しておいてください
- VoiSona Talkは操作しないでください
- VoiSona Talkには既存のプロジェクトを読み込ませないでください
- VoiSona Talkはダイアログやサブメニューを出しっぱなしにしないでください
- VoiSona TalkのUIの言語は日本語にしておいてください

## できること

- セリフをYMM4上から合成する
- ボイスライブラリの切替
  - 選択肢は「YMM4 VoiSona Talkプラグインの声質を再読み込み」で取得必要
- プラグインの更新確認とダウンロード

## できないこと

### 将来的に対応予定

- 高速なセリフ合成
  - 現在は 2~4 秒程度かかります
- セリフのグローバルパラメータ対応
  - Speed, Volume, Pitch, Alpha, Into., Hus.
- セリフのプリセット対応
- セリフのスタイル対応
- 日本語以外のUI言語対応（VoiSona Talk）
- 別方式のセリフ合成（音声キャプチャ）

### 対応予定なし

- 辞書登録

### 対応未定

- アクセント、STY、VOL、PIT、ALP、HUS
- VoiSona（Song）

## License

- [/licenses](./licenses/)
  - SonaBridge - MIT
  - FlaUI - MIT
  - Epoxy - Apache-2.0 license
