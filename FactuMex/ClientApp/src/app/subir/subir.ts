export class Subir {
    public constructor(
        public IDArchivo: number,
        public BinaryString: string,
        public Base64Url: string,
        public MimeType: string,
        public Nombre: string,
        public IDDescripcion: number,
        public Tamano: number,
        public IDEntidad: number,
        public PKEntidad: number,
        public Ruta: string
    ) {
    }
}
export class SubirInfo {
    public constructor(
        public IDEntidad: number,
        public PKEntidad: number
    ) {
    }
}