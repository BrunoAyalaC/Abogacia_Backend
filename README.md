# LegalPro Backend

Backend para la plataforma LegalPro - Soluciones legales inteligentes respaldadas por IA.

## 📁 Estructura

```
├── legalpro-app/          → Backend Express 5 (Node.js)
│   ├── server/            → API endpoints y lógica
│   └── src/               → Frontend React SPA
│
└── LegalProBackend_Net/   → Backend ASP.NET Core 9
    ├── LegalPro.Api/
    ├── LegalPro.Application/
    ├── LegalPro.Domain/
    └── LegalPro.Infrastructure/
```

## 🚀 Stack Tecnológico

- **Express 5** - API REST (Node.js)
- **ASP.NET Core 9** - Clean Architecture + CQRS
- **PostgreSQL** - Base de datos (Supabase Cloud)
- **Supabase** - Auth + Storage + Realtime
- **Google Gemini** - IA generativa para análisis legal
- **Railway** - Deployment cloud

## 🔧 Configuración Rápida

### Express (legalpro-app)

```bash
cd legalpro-app
npm install
npm run dev  # Puerto 3000
```

### .NET (LegalProBackend_Net)

```bash
cd LegalProBackend_Net
dotnet restore
dotnet run --project LegalPro.Api  # Puerto 5000
```

## 🌍 Variables de Entorno

```env
SUPABASE_URL=https://xxx.supabase.co
SUPABASE_KEY=eyJ...
GEMINI_API_KEY=AIza...
JWT_SECRET=super-secret-key
DATABASE_URL=postgresql://...
```

## 📚 Documentación

- **Express Routes**: legalpro-app/server/routes/
- **Entity Framework**: LegalProBackend_Net/LegalPro.Infrastructure/
- **CQRS Handlers**: LegalProBackend_Net/LegalPro.Application/

## 🚢 Despliegue

Ambos backends se despliegan en **Railway**:

```bash
# Express
railroad link --service=legalpro-app
railroad up

# .NET
# Conectar GitHub repo - Railway detecta automáticamente
```

## 📝 Endpoints Principales

- `GET /api/expedientes` - Listar expedientes
- `POST /api/auth/login` - Autenticación
- `POST /api/gemini/analizar` - Análisis IA
- `PUT /api/expedientes/:id` - Actualizar expediente

## 📞 Contacto

Para preguntas sobre el backend, revisa la documentación en `.github/instructions/`
