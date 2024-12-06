<!-- PROJECT LOGO -->
<br />
<div align="center">
  <img src="images/logo.jpg" style="display: block;  margin-left: auto;  margin-right: auto;  width: 45%;">
  <h3 align="center">README.MD</h3>

  <p align="center">
    Aquest repositori conté el codi font de la solució amb els projectes de Codis365, accessible des l'enllaç https://backend.codis365.es
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
## Taula de Continguts

1. [Introducció](#introducció)
2. [Arquitectura de la Solució](#arquitectura-de-la-solució)
   - [PresentationLayer](#presentationlayer)
   - [ExternalAPI](#externalapi)
   - [Entities](#entities)
   - [DataAccessLayer](#dataaccesslayer)
   - [BusinessLogicLayer](#businesslogiclayer)
   - [Resources](#resources)
   - [iTests](#itests)
3. [Requisits](#requisits)
4. [Instal·lació i Configuració](#instal·lació-i-configuració)
5. [Execució](#execució)
   - [Mode Local](#mode-local)
   - [Mode de Producció](#mode-de-producció)
6. [Estructura del Codi](#estructura-del-codi)
7. [Proves](#proves)
8. [Contribució](#contribució)
9. [Llicència](#llicència)

<!-- DESCRIPTION -->
## Descripció
<div id="description"></div>

Aquesta pàgina és una pàgina web independent creada específicament per a una campanya de màrqueting o publicitat. És on aterra un visitant després de fer clic en un enllaç en un correu electrònic, anuncis de Google, Bing, YouTube, Facebook, Instagram, Twitter o llocs similars a la web. A diferència de les pàgines web, que solen tenir molts objectius i fomentar l'exploració, les pàgines d'aterratge estan dissenyades amb un únic focus o objectiu, conegut com a crida a l'acció (CTA). Aquest enfocament fa que les pàgines d'aterratge siguin la millor opció per augmentar les taxes de conversió de les teves campanyes de màrqueting i reduir el teu cost d'adquirir un lead o venda. 

>👉 CODIS365 ofereix un complet conjunt de solucions de codi de barres destinades a transformar els negocis a través de la creació avançada i personalitzable de codis de barres, proves i validació en temps real, i funcions de gestió a granel. Aprofita la realitat augmentada per a les vistes prèvies de col·locació de codis de barres i ofereix més de 40 simbologies úniques de codis de barres. La plataforma està dissenyada per millorar la gestió de l'inventari, oferint funcions com gràfics dinàmics per al seguiment del rendiment, integració perfecta de l'API i suport avançat per a la gestió eficient de l'inventari i la presa de decisions basada en dades. Les opcions de preus s'adapten a les diferents necessitats de les empreses, des de serveis bàsics fins a serveis premium.

>⚠️Totes les dades esmentades en aquesta pàgina d'aterratge són fictícies i representen un mitjà simulat d'accés al backend o sistema de gestió de Codis365, ja sigui mitjançant l'opció "Login" per a la identificació d'un usuari existent, o a través de "SignUp" per a la creació d'un nou usuari.

<!-- ACCESSIBILITY -->
## Consideracions d'accessibilitat
<div id="accessibility"></div>

Aquest projecte compleix amb tots els estàndards d'accessibilitat de les WCAG 2.0 i s'ha verificat a través d'eines del W3C (World Wide Web Consortium) com ara "W3C CSS Validation Service" o "Web Content Accessibility Guidelines (WCAG) 2.2 Level AA Conformance".

<div id="block" align="center">
    <div class="inline-block" style="display: inline-block; width: 20%">
        <img src="images/wacg-1.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 20%">
        <img src="images/wacg-2.png">
    </div>
    <div class="inline-block" style="display: inline-block; width: 20%">
        <img src="images/wacg-3.png">
    </div>
</div>


<!-- SCREENSHOTS -->
## Captures de pantalla
<div id="screenshots"></div>

A continuació es mostren algunes captures de pantalla de la pàgina d'aterratge, tant en la versió d'escriptori com en la versió mòbil.

<div id="block" align="center">
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-1.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-2.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-3.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-4.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-5.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-6.png">
    </div>
    <br>
    <div class="inline-block" style="display: inline-block; width: 25%">
        <img src="images/landingpage-7.png">
    </div>
</div>
