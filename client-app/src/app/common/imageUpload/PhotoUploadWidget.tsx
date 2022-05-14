import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Button, Grid, Header } from "semantic-ui-react";
import PhotoWidgetCroppet from "./PhotoWidgetCroppet";
import PhotoWidgetDropzone from "./PhotoWidgetDropzone";

interface Props {
    loading: boolean;
    uploadPhoto: (file: Blob) => void;
}


export default observer( function PhotoUploadWidget({loading, uploadPhoto} : Props) {
    const [files, setFiles] = useState<any>([]);
    const [croppper, setCropper] = useState<Cropper>();

    function onCrop() {
        if(croppper) {
            croppper.getCroppedCanvas().toBlob(blob => uploadPhoto(blob!));
        }
    }

    useEffect(() => {
        return () => {
            files.forEach((file: any) => URL.revokeObjectURL(file.preview));
        }
    }, [files])

    return(
        <Grid>
            <Grid.Column width={4}>
                <Header color="teal" content='Step 1 - Add photo' />
                <PhotoWidgetDropzone setFiles={setFiles}/>
            </Grid.Column>
            <Grid.Column width={1} />
            <Grid.Column width={4}>
                <Header color="teal" content='Step 2 - Resize photo' />
                {files && files.length > 0 && (
                    <PhotoWidgetCroppet setCropper={setCropper} imagePreview={files[0].preview}/>
                )}
            </Grid.Column>
            <Grid.Column width={1} />
            <Grid.Column width={4}>
                <Header color="teal" content='Step 3 - Priview & upload' />
                {files && files.length > 0 &&
                <>
                    <div className="img-preview" style={{minHeight: 200, overflow: 'hidden'}}/>
                    <Button.Group widths={2}>
                        <Button loading={loading} onClick={onCrop} positive icon='check'/>
                        <Button disabled={loading} onClick={() => setFiles([])} icon='close'/>
                    </Button.Group>
                </>}
            </Grid.Column>
        </Grid>
    )
})