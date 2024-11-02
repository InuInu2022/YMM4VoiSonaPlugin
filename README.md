# YMM4VoiSonaPlugin

![cover](https://github.com/InuInu2022/YMM4VoiSonaPlugin/blob/main/docs/images/ymm4voisonatalkpluginthum.png?raw=true)

## これはなに？

YMM4のボイスとして **「VoiSona Talk」(ボイソナトーク)** を使えるようにしたプラグインです。

現在開発中のため、台詞に細かいパラメータを指定できません。
台詞の合成にやや時間が掛かります。
また、たまに失敗することがあります。

## インストール方法

[Releases](https://github.com/InuInu2022/YMM4VoiSonaPlugin/releases) 以下にある最新のバージョンの`YMM4VoiSonaPlugin.ymme`をインストールしてください。

「キャラクター設定」の「ボイス」で、「YMM4 VoiSona Talkプラグインの声質を再読み込み」を選択して、現在のボイスライブラリを取得してください。
※新しくボイスライブラリを増やすたびに必要です

## ニコニコモンズ

ニコニコに投稿する際には以下のコンテンツIDを親子登録してください。

[nc375085](https://commons.nicovideo.jp/works/nc375085)

(YMM4の素材一覧からも確認できます。)

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
![ss](https://github.com/InuInu2022/YMM4VoiSonaPlugin/blob/main/docs/images/YMM4VoiSonaPlugin_download.png?raw=true)
- セリフのグローバルパラメータ対応 (v0.2)
  - Speed, Volume, Pitch, Alpha, Into., Hus.
- セリフのスタイル対応 (v0.3)

## できないこと

### 将来的に対応予定

- 高速なセリフ合成
  - 現在は 2~4 秒程度かかります
- セリフのプリセット対応
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
  - Material.Icons.WPF - MIT
