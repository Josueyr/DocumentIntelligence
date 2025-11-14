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

Actualmente, existe un módulo de **recibos** que permite enviar documentos y recuperar **datos estructurados**.

---

## 🧪 Desarrollo y pruebas

- Añadir nuevas pruebas **unitarias** y de **integración** según se vaya extendiendo la lógica.  
- Utilizar **inyección de dependencias** para facilitar testing y desacoplamiento de implementaciones reales de Azure.  

---

## 🔒 Buenas prácticas de seguridad

- No subir claves ni secretos al repositorio.  
- En producción usar **Azure Key Vault** o un gestor de secretos.  
- Proteger la API con **autenticación/autorización** si se expone públicamente.  

---

## 🤝 Contribuir

1. Hacer **fork** del repositorio.  
2. Crear una rama `feature/` o `fix/`.  
3. Abrir un **Pull Request** con descripción clara y tests si aplica.  

---

## 📜 Licencia

Añadir un archivo `LICENSE` en la raíz con la licencia deseada (por ejemplo **MIT**).

---

## 📬 Contacto

Abrir un **issue** en el repositorio para preguntas o reportes de bugs.
