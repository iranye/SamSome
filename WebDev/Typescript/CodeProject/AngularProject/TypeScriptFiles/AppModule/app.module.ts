// Here are the steps for creating such Root Component.

// Create Angular Module
// Create Angular Component
// Add Component to Module
// Bootstrap Module
// Bootstrap Component

import {NgModule} from "@angular/core"
import {AppComponent} from "./app.component"
@NgModule({
    declarations:[AppComponent],
    bootstrap:[AppComponent]
})
export class AppModule{

}