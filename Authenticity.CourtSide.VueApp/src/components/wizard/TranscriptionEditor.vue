<template>
  <v-container class="selector-container">
    <div v-show="isOnEdition" class="transcription-editor-container">
      <p
        class="transcription-editor"
        
      :style="transcriptEditorReactiveStyle"
        contenteditable
        ref="textEditor"
        @keydown.enter.prevent="saveText"
        @keyup.esc.prevent="onClose"
        @keydown.tab.prevent="moveCursor"
        @keyup.right="moveCursorArrow"
        @keyup.left="moveCursorArrow"
        v-closable="{
          exclude: ['select', 'chgAll', 'chgOne', 'triggerEditor'],
          handler: 'onClose',
        }"
        @mouseup="highLightWord()"
      ></p>
    </div>
    <p
      ref="transcription"
      v-show="!isOnEdition"
      align="left"
      class="chunk-label-read-only transcript-editor-read-only"
      v-on:dblclick="editText()"
    >
      {{ dialog.transcription }}
    </p>
  </v-container>
</template>

<script>
import { mapMutations, mapState } from "vuex";

export default {
  name: "TranscriptionEditor",
  props: {
    dialogPosition: {
      type: Number,
      required: true,
    },
    dialog: {
      type: Object,
      required: true,
    },
  },
  data: () => {
    let isOnEdition = false;
    let defaultEditorHeight = 60;
    return {
      isOnEdition,
      newText: "",
      defaultEditorHeight,
      spanEnd: "",
      highLightedWordClass: "cs-highlighted-word",
    };
  },
  computed: {
    ...mapState("transcript", ["transcriptInEdition"]),
    textEditor() {
      return this.$refs.textEditor;
    },
    transcriptEditorReactiveStyle() {
      return { height: this.defaultEditorHeight + 'px'};
    }
  },
  methods: {
    ...mapMutations("transcript", ["setTranscriptInEdition"]),
    onClose() {
      this.isOnEdition = false;
      this.setTranscriptInEdition(false);
    },
    editText() {
      this.newText = this.dialog.transcription;
      this.isOnEdition = true;
      this.setTranscriptInEdition(true);
      this.defaultEditorHeight = this.$refs.transcription.scrollHeight + 30;

      setTimeout(() => {
        this.textEditor.focus();
        const wordsList = this.newText.split(/\s{1}/);

        for (let i = 0; i < wordsList.length; i++) {
          wordsList[i] = `<span id="word-${i}">${wordsList[i]} </span>`;
        }

        this.textEditor.innerHTML = wordsList.join(" ");
        this.textEditor.childNodes[0].classList.add(this.highLightedWordClass);
      }, 10);
    },
    saveText() {
      this.isOnEdition = false;
      this.setTranscriptInEdition(false);
      this.newText = this.textEditor.innerText;
      this.newText = this.newText.replace(/(\r\n|\n|\r)/gm, "");
      if (this.newText != this.dialog.transcription) {
        this.dialog.transcription = this.newText;
        this.$emit("saveTranscription", this.dialogPosition, this.dialog);
      }
    },
    moveCursor(event) {
      var selection;
      if (window.getSelection && (selection = window.getSelection()).modify) {
        var selectedRange = selection.getRangeAt(0);
        let wordNode = selectedRange.startContainer.parentNode;

        if (event.shiftKey) {
          wordNode = this.moveCursorShiftTab(wordNode, selection);
        } else {
          wordNode = this.moveCursorTab(wordNode);
        }

        if (wordNode) {
          var range = document.createRange();
          range.setStart(wordNode, 1);
          range.collapse(true);
          selection.removeAllRanges();
          selection.addRange(range);
        }
      }
    },
    moveCursorTab(wordNode) {
      if (wordNode && wordNode.tagName.toLowerCase() === "span") {
        const nextWordNode = wordNode.nextElementSibling;
        this.setHighLightWordClass(nextWordNode);
      } else {
        wordNode = this.getHighLightedWordElement();
        const nextWordNode = wordNode.nextElementSibling;
        this.setHighLightWordClass(nextWordNode);
      }
      return wordNode;
    },
    moveCursorShiftTab(wordNode, selection) {
      wordNode = this.getHighLightedWordElement();
      const previousNode = wordNode.previousElementSibling;
      const firstWordNode = this.textEditor.childNodes[0];

      if (previousNode && previousNode.id !== firstWordNode.id) {
        this.setHighLightWordClass(previousNode);
        wordNode = previousNode.previousElementSibling;
      } else {
        wordNode = null;
        this.setHighLightWordClass(firstWordNode);
        var range = document.createRange();
        range.setStart(firstWordNode, 0);
        range.collapse(true);
        selection.removeAllRanges();
        selection.addRange(range);
      }

      return wordNode;
    },
    moveCursorArrow() {
      var selection;
      if (window.getSelection && (selection = window.getSelection()).modify) {
        var selectedRange = selection.getRangeAt(0);
        let wordNode = selectedRange.startContainer.parentNode;
        this.setHighLightWordClass(wordNode);
      }
    },
    highLightWord() {
      var sel;
      if (window.getSelection && (sel = window.getSelection()).modify) {
        var selectedRange = sel.getRangeAt(0);
        sel.collapseToStart();
        sel.modify("move", "backward", "word");
        sel.modify("extend", "forward", "word");
        const wordElement = selectedRange.startContainer.parentNode;
        this.setHighLightWordClass(wordElement);
        sel.removeAllRanges();
        sel.addRange(selectedRange);
      }
    },
    setHighLightWordClass(element) {
      if (element) {
        document
          .getElementsByClassName(this.highLightedWordClass)
          .forEach((word) => {
            word.classList.remove(this.highLightedWordClass);
          });
        element.classList.add(this.highLightedWordClass);
      }
    },
    getHighLightedWordElement() {
      return document.getElementsByClassName(this.highLightedWordClass)[0];
    },
  },
  watch: {
    isOnEdition(val) {
      this.$emit("onEnableScroll", !val);
    }
  },
  mounted() {
    this.setTranscriptInEdition(false);
  },
};
</script>