import {
  ChatClient,
  ChatThreadClient,
  ChatMessageReceivedEvent
} from "@azure/communication-chat";
import {
  AzureCommunicationTokenCredential
} from "@azure/communication-common";

export class CommandChatClient {
  private chatClient: ChatClient;
  private chatThreadClient!: ChatThreadClient;
  private userId: string;

  constructor(
    private endpointUrl: string,
    private token: string,
    private threadId: string,
    userId: string
  ) {
    this.userId = userId;
    const credential = new AzureCommunicationTokenCredential(token);
    this.chatClient = new ChatClient(endpointUrl, credential);
  }

  async initialize(onCommandReceived: (command: string, rawEvent: ChatMessageReceivedEvent) => void): Promise<void> {
    this.chatThreadClient = await this.chatClient.getChatThreadClient(this.threadId);
    await this.chatClient.startRealtimeNotifications();

     this.chatClient.on("chatMessageReceived", (event) => {
      if (event.threadId !== this.threadId) return; // only handle this thread
      const message = event.message;
      if (message) {
        onCommandReceived(message, event);
      }
    });
  }

  async sendCommand(command: string): Promise<void> {
    if (!this.chatThreadClient) {
      throw new Error("Chat thread client not initialized. Call initialize() first.");
    }

    await this.chatThreadClient.sendMessage({
      content: command
    });
  }
}
