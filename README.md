# 🧠 DocumentIntelligence
**API .NET para extraer y estructurar información de recibos y facturas usando NLP y OCR.**

Proyecto en **.NET 9** para extracción y procesamiento de información de documentos (recibos, facturas y otros) utilizando servicios de IA como **Azure OpenAI** y **Cognitive Services**.

---

## 📄 Descripción

Este repositorio contiene una **API** y módulos especializados para analizar documentos mediante **OCR** y **NLP**, devolviendo **datos estructurados** listos para su uso en aplicaciones de contabilidad, facturación o gestión documental.  
El desarrollo sigue principios de **Clean Code** y está organizado bajo un enfoque de **Vertical Slicing** para facilitar mantenibilidad y escalabilidad.

---

## 🏗 Estructura del repositorio

- **`DocumentIntelligence.Api`** — API web (ASP.NET Core) que expone endpoints para subir documentos y obtener resultados.  
- **`DocumentIntelligence.Modules.Receipt`** — Módulo con la lógica de extracción y normalización de campos de recibos.

---

## ✨ Características principales

- Procesamiento de documentos mediante **OCR** y **modelos de lenguaje**.  
- Extracción de campos estructurados: **importe, fecha, proveedor, items, impuestos**, etc.  
- Arquitectura modular, diseñada para **extenderse con nuevos módulos** (facturas, contratos, identificaciones).  
- Integración con servicios de **Azure**: OpenAI y Cognitive Services.  
- Autenticación mediante **JWT** para asegurar los endpoints y controlar el acceso a los servicios.

---

## ⚙ Requisitos

- **.NET 9 SDK**  
- **Cuenta de Azure**  

---

## 🛠 Configuración

Se recomienda usar **Azure Key Vault** en producción.  
Ejemplo mínimo para realizar pruebas en `DocumentIntelligence.Api/appsettings.json`:

```json
{
  "AzureSettings": {
    "Endpoint": "https://<your-resource>.cognitiveservices.azure.com/",
    "ApiKey": "<your-key>"
  }
}
```

---

## 🚀 Probar un endpoint

La API incluye **Swagger** para probar los endpoints de manera interactiva.

---

## 🛠 Endpoints

Revisar el código de `DocumentIntelligence.Api` para conocer:

- Rutas exactas  
- Modelos de solicitud y respuesta  


