## テストについてメモ
MSTest では、テストはテスト名の順序に自動的に設定されます



## Injectionのライフサイクル
### AddTransient
有効期間が一時的なサービス (AddTransient) は、サービス コンテナーから要求されるたびに作成されます。 この有効期間は、軽量でステートレスのサービスに最適です。
要求を処理するアプリでは、一時的なサービスが要求の最後に破棄されます

### AddScoped
有効期間がスコープのサービス (AddScoped) は、クライアント要求 (接続) ごとに 1 回作成されます。
要求を処理するアプリでは、スコープ付きサービスは要求の最後で破棄されます。

### AddSingleton
有効期間がシングルトンのサービス (AddSingleton) は、最初に要求されたときに作成されます (または、Startup.ConfigureServices が実行されて、サービス登録でインスタンスが指定された場合)。 以降の要求は、すべて同じインスタンスを使用します。 アプリをシングルトンで動作させる必要がある場合は、サービス コンテナーによるサービスの有効期間の管理を許可することをお勧めします。 クラス内のオブジェクトの有効期間を管理するために、シングルトンの設計パターンを実装してユーザー コードを提供しないでください。
要求を処理するアプリでは、ServiceProvider がアプリのシャットダウン時に破棄されたとき、シングルトン サービスが破棄されます。
