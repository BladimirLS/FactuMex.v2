import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { LoginComponent } from './login/login.component';
import { PopupComponent } from './popup/popup.component';
import { ProductoComponent } from './producto/producto.component';
import { ComprobanteComponent } from './comprobante/comprobante.component';
import { ConfigComponent } from './config/config.component';
import { ImportarComponent } from './importar/importar.component';
import { ClienteComponent } from './cliente/cliente.component';
import { vPreviaComponent } from './vprevia/vprevia.component';
import { UsuarioComponent } from './usuario/usuario.component';
import { LstclienteComponent } from './lstcliente/lstcliente.component';
import { LstproductoComponent } from './lstproducto/lstproducto.component';
import { FacturaComponent } from './factura/factura.component';
import { ValijaComponent } from './valija/valija.component';
import { TasaComponent } from './tasa/tasa.component';
import { Servicio } from './Service';
import {FactmexService} from './Service/factmex.service';

// @ts-ignore
// @ts-ignore
// @ts-ignore
@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        LoginComponent,
        ClienteComponent,
        vPreviaComponent,
        PopupComponent,
        ImportarComponent,
        UsuarioComponent,
        ConfigComponent,
        ProductoComponent,
        ComprobanteComponent,
        LstclienteComponent,
        LstproductoComponent,
        FacturaComponent,
        ValijaComponent,
        TasaComponent
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', component: HomeComponent, pathMatch: 'full' },
            { path: 'cliente', component: ClienteComponent },
            { path: 'cliente/:id', component: ClienteComponent },
            { path: 'lstcliente', component: LstclienteComponent },
            { path: 'producto', component: ProductoComponent },
            { path: 'producto/:id', component: ProductoComponent },
            { path: 'lstproducto', component: LstproductoComponent },
            { path: 'comprobante', component: ComprobanteComponent },
            { path: 'factura', component: FacturaComponent },
            { path: 'factura/:id', component: FacturaComponent },
            { path: 'importar', component: ImportarComponent },
            { path: 'usuario', component: UsuarioComponent },
            { path: 'config', component: ConfigComponent },
            { path: 'valija', component: ValijaComponent },
            { path: 'tasa', component: TasaComponent },
        ])
    ],
    providers: [
        Servicio,
        FactmexService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
