interface ILogger {
    LogError(e:string):void;
    SendEmailLog(s:string):boolean;
}

abstract class AbstractServiceInvoker {
    Save() {
        let ServiceURL:string=this.GetServiceURL();
        let Credentials="UserName:abcd,password:123";
        let data:string=this.getData();

        let Result:number=5;
        this.setData(Result);
    }
    abstract GetServiceURL():string;
    abstract getData():string;
    abstract setData(n:number):void;
}

class Logger implements ILogger {
    LogError(e:string):void {
        console.log(`error ${e} logged`);
    }
    SendEmailLog(s:string):boolean {
        console.log(`message ${s} sent via email`);
        return true;
    }
}

class CustomerService extends AbstractServiceInvoker {
    GetServiceURL(): string {
        return "http://someService.com/abcd"
    }
    getData(): string {
        return "{CustomerName:'a', Address:'b'}"
    }
    setData(n: number): void {
        throw new Error("Method not implemented.");
    }
}