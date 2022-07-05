import { Formik, Form, Field, FieldProps } from 'formik';
import { observer } from 'mobx-react-lite'
import { useEffect } from 'react'
import { Link } from 'react-router-dom';
import {Segment, Header, Comment, Loader} from 'semantic-ui-react'
import { useStore } from '../../../../app/stores/store';
import * as Yup from 'yup';
import { formatDistanceToNow } from 'date-fns';

interface Props {
    activityId : string;
}

export default observer(function ActivityDetailedChat({activityId} : Props) {
    const {commentStore, userStore :{user}} = useStore();

    useEffect(() => {
        if(activityId) {
            commentStore.createHubConnection(activityId);
        }
        return () => {
            commentStore.clearComments();
        }
    }, [commentStore, activityId]);

    return (
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{border: 'none'}}
            >
                <Header>Chat about this crypto</Header>
            </Segment>
            <Segment attached clearing>
            <Formik 
                        onSubmit={(values, {resetForm}) => 
                            commentStore.addComments(values).then(() => resetForm())}
                            initialValues={{body: ''}}
                            validationSchema = {Yup.object({
                                body: Yup.string().required()
                            })}
                    >
                        {({isSubmitting, isValid, handleSubmit}) => (
                        <Form className='ui form'>
                            <Field name='body'>
                                {(props: FieldProps) => (
                                    <div style={{position: 'relative'}}>
                                        <Loader active={isSubmitting} />
                                        <textarea 
                                            disabled={user?.IsBanned}
                                            placeholder= {!user?.IsBanned ? 'Enter your comment (Enter to submit, Shift + Enter for new line)' 
                                            : 'You have been banned due to non-compliance with the user agreement'}
                                            rows={2}
                                            {...props.field}
                                            onKeyPress={e => {
                                                if (e.key === 'Enter' && e.shiftKey){
                                                    return;
                                                }
                                                if (e.key === 'Enter' && !e.shiftKey){
                                                    e.preventDefault();
                                                    isValid && handleSubmit();
                                                }
                                            }}
                                        />
                                    </div>
                                )}
                            </Field>
                        </Form>
                        )}
                    </Formik>
                <Comment.Group>
                    {commentStore.comments.map(comment => (
                        <Comment key={comment.id}>
                            <Comment.Avatar src={comment.image || '/assets/user.png'}/>
                            <Comment.Content>
                                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>
                                    {comment.displayName}
                                    </Comment.Author>
                                <Comment.Metadata>
                                    <div>{formatDistanceToNow(comment.createdAt)} ago</div>
                                </Comment.Metadata>
                                <Comment.Text style={{whiteSpace: 'pre-wrap'}}>{comment.body}</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    ))}

                </Comment.Group>
            </Segment>
        </>
    )
})
