# VPN-AutoConnect
[Passlogic](https://www.passlogy.com/)の
[トークンレス・ワンタイムパスワード](https://passlogic.jp/tokenless/)と、
[CISCO AnyConnect](https://www.cisco.com/c/ja_jp/products/security/anyconnect-secure-mobility-client/index.html)
を組み合わせたVPN接続先への自動ログインツールです。

![image](https://user-images.githubusercontent.com/25922944/81464880-5f9cfa80-9200-11ea-9d1f-832c902ab9fb.png)

## 機能
1. トークンレス・ワンタイムパスワードの自動取得
* あらかじめパターンを入力しておくことで、PassLogicのWebサイトからのテーブル取得と
パスフレーズ作成を自動化します。
* セキュリティに配慮し、パターンは設定ファイルに記載せず、本ツールを立ち上げる際に一度だけ入力を促し、
ツール終了後は破棄されます。
* PassLogicのURLには、2つのカスタムIDフィールドを埋め込むことができます。
セキュリティに配慮し、カスタムID値は設定ファイルに記載せず、本ツールを立ち上げる際に一度だけ入力を促し、
ツール終了後は破棄されます。

2. VPN接続の自動化
* Cisco AnyConnectコマンドラインツールを起動し、VPN接続を自動運転します。
* VPN接続に必要なパスワードは、自動取得したトークンレス・ワンタイムパスワードが用いられます。

3. VPN接続の監視
* VPN接続後は、VPN接続状態を監視し、接続断となった場合は自動的に再接続を行います。

4. カスタムコマンド起動の自動化
* あらかじめ登録しておいたコマンドをワンクリックで起動する機能があります。VPN接続後の、
仮想デスクトップ接続操作など任意の操作を簡単に行うことができます。
* カスタムコマンドには、2つのカスタムIDフィールドを埋め込むことができます。
セキュリティに配慮し、カスタムID値は設定ファイルに記載せず、本ツールを立ち上げる際に一度だけ入力を促し、
ツール終了後は破棄されます。

5. GUIカスタマイズ機能
* カスタムIDを入力するフィールドラベル及び、カスタムコマンドボタンの表示をカスタマイズすることができます。


## インストール
* [Binaryパッケージ](https://github.com/tomoyukioya/VPN-AutoConnect/releases)を取得した場合は、
適当なフォルダを展開し、「VpnAutoConnect.exe」を起動してください。
* ソースからビルドを行う場合は、「App.config.template」を「App.config」にコピーしてから、
Visual Studio 2019でビルドを行ってください。

## UIのカスタマイズ
Binaryパッケージの場合は「VpnAutoConnect.exe.config」を、
ソースからビルドを行う場合は「App.config」を編集してください。
key=LabelId1, LabelId2, LabelCustomCommandButtonの各フィールドを変更することにより、
対応するラベル及びテキストを変更することができます。

```xml:VpnAutoConnect.exe.config / App.config
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7.2" />
  </startup>
  <appSettings>
    <add key="LabelId1" value="コード１"/>
    <add key="LabelId2" value="コード２"/>
    <add key="LabelCustomCommandButton" value="カスタム"/>

    <add key="PassLogicUrl" value="https://XXXXX.co.jp/passlogic/ui/keyreq.php?id={ID}%40YYYY.ZZZZ.net&amp;kn=9999"/>
    <add key="AnyConnectCliPath" value="C:\Program Files (x86)\Cisco\Cisco AnyConnect Secure Mobility Client\vpncli.exe"/>
    <add key="CustomCommand" value="C:\Program Files (x86)\VMware\VMware Horizon View Client\vmware-view.exe"/>
    <add key="CustomCommandParameters" value="--serverURL XXX.YYY.com --password {ID2} --userName {ID1}@YYYY.ZZZZ.net --desktopName 仮想デスクトップ"/>
  </appSettings>
</configuration>
```

## 使用方法
1. 「パスフレーズ登録」ボタンを押した後、
PassLogicトークンレス・ワンタイムパスワードのパネルを順番にクリックしてパタンを登録してください。
「登録終了」ボタンを押すと登録が完了となります。
（パスフレーズが登録済みになると、ボタンのテキストの末尾に「*」が表示されます。
再度「パスフレーズ登録」ボタンを押下することにより、パスフレーズを上書き登録することができます。）

2. 歯車ボタン
![image](https://user-images.githubusercontent.com/25922944/81465296-0767f780-9204-11ea-91f4-6565e3dcbba0.png)
を押下し、各種設定を確認してください。
![image](https://user-images.githubusercontent.com/25922944/81465280-e99a9280-9203-11ea-9493-07d1b4070a89.png)

|フィールド|説明|
|:--|:--|
|PassLogicキーのURL|PassLogicのワンタイムパスワードテーブルを取得するURLを記載してください。文字列中に「\{ID1}」、「\{ID2}」が現れた場合、それらは、それぞれ、GUIのLabelId1, LabelId2のテキストボックスに入力された文字列に置換されます。|
|AnyConnect CLIのインストールパス|vpncli.exeがインストールされているフルパス名を記載してください。|
|カスタムコマンド|GUIのカスタムコマンドボタンで起動する実行ファイルのフルパス名を記載してください。|
|カスタムコマンド引数|カスタムコマンドに与える引数を記載してください。文字列中に「\{ID1}」、「\{ID2}」が現れた場合、それらは、それぞれ、GUIのLabelId1, LabelId2のテキストボックスに入力された文字列に置換されます。|


3. 「コード１」、「コード２」にユーザ名、パスワードなどの文字列を入力してください。

4. 「VPN接続」ボタン押下で、VPN接続が行われます。実行状況は「Log」ボタンで確認できます。

5. VPN接続後、「カスタム」ボタンで、任意のコマンドを実行することができます。
仮想デスクトップ接続を行うなどの定型操作に利用可能です。

6. 「VPN切断」ボタンでVPN接続を切断します。

## 注意
* 自動VPN接続に先立ち、本ツールはui版のAnyConnect(vpnui.exe)を終了させます。 再度ui版のVPN接続を行う場合は、
手動でvpnui.exeを起動してください。
