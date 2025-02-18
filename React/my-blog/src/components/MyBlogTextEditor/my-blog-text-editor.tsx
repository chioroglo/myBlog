import { useCallback, useEffect, useRef, useState } from "react"
import {
  Editor,
  EditorState,
  RichUtils,
  EditorCommand,
  ContentBlock,
  convertFromHTML,
  ContentState,
} from "draft-js";
import { TextEditorToolbar } from "./TextEditorToolbar";
import styles from "./my-blog-text-editor.module.scss";
import { stateToHTML } from "draft-js-export-html";

const PLACEHOLDER_TEXT_EDITOR = "✍️ Jot your thoughts here..."
const POST_MAX_LENGTH = 10000;

export interface MyBlogTextEditorProps {
  defaultContent: string,
  onChange: (value: string) => void
}

const MyBlogTextEditor = ({
  defaultContent = "",
  onChange = () => {} } : Partial<MyBlogTextEditorProps>) => {

  const blocksFromHtml = convertFromHTML(defaultContent);
  const contentState = ContentState.createFromBlockArray(
    blocksFromHtml.contentBlocks,
    blocksFromHtml.entityMap
  );
  const [editorState, setEditorState] = useState(EditorState.createWithContent(contentState));

  const editorRef: React.LegacyRef<Editor> = useRef(null);

  useEffect(() => {
    focusEditor();
  }, [editorRef]);

  const focusEditor = useCallback(() => {
    if (editorRef.current) {
      editorRef.current?.focus();
    }
  }, [editorRef]);

  const handleKeyCommand = (command : EditorCommand) => {
    const newState = RichUtils.handleKeyCommand(editorState, command);
    if (newState) {
      setEditorState(newState);
      return true;
    }
    return false;
  };

  // FOR INLINE STYLES
  const styleMap = {
    CODE: {
      backgroundColor: "rgba(0, 0, 0, 0.05)",
      fontFamily: '"Inconsolata", "Menlo", "Consolas", monospace',
      fontSize: 16,
      padding: 2,
    },
    HIGHLIGHT: {
      backgroundColor: "#F7A5F7",
    },
    UPPERCASE: {
      textTransform: "uppercase",
    },
    LOWERCASE: {
      textTransform: "lowercase",
    },
    CODEBLOCK: {
      fontFamily: '"fira-code", "monospace"',
      fontSize: "inherit",
      background: "#ffeff0",
      fontStyle: "italic",
      lineHeight: 1.5,
      padding: "0.3rem 0.5rem",
      borderRadius: " 0.2rem",
    },
    SUPERSCRIPT: {
      verticalAlign: "super",
      fontSize: "80%",
    },
    SUBSCRIPT: {
      verticalAlign: "sub",
      fontSize: "80%",
    }
  };

  // FOR BLOCK LEVEL STYLES(Returns CSS Class From DraftEditor.css)
  const myBlockStyleFn = (contentBlock : ContentBlock) => {
    const type = contentBlock.getType();
    switch (type) {
      case "blockQuote":
        return "superFancyBlockquote";
      case "leftAlign":
        return "leftAlign";
      case "rightAlign":
        return "rightAlign";
      case "centerAlign":
        return "centerAlign";
      case "justifyAlign":
        return "justifyAlign";
      default:
        break;
    }
  };


  return (
    <div className={styles["editor-wrapper"]}>
      <TextEditorToolbar editorState={editorState} setEditorState={setEditorState} />
      <div className={styles["editor-container"]}>
        <Editor
          ref={editorRef}
          placeholder={PLACEHOLDER_TEXT_EDITOR}
          handleKeyCommand={handleKeyCommand}
          editorState={editorState}
          customStyleMap={styleMap}
          blockStyleFn={myBlockStyleFn}
          onChange={(editorState) => {
            const currentContent = editorState.getCurrentContent();
            const plainText = currentContent.getPlainText();
            if (plainText.length > POST_MAX_LENGTH) {
              return;
            }
            setEditorState(editorState);
            onChange(stateToHTML(currentContent));
          }}
        />
      </div>
    </div>
  );
}

export {MyBlogTextEditor}